using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public interface IAuthService
{
    Task<LoginResponseDto> Register(RegistrationDto registration);
    Task<LoginResponseDto> Login(LoginCredentialsDto loginCredentials);
    Task<LoginResponseDto> RefreshToken();
    Task Logout();
}

public class AuthService : IAuthService
{
    private readonly StarDb dbcontext;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration configuration;

    public AuthService(StarDb dbcontext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        this.dbcontext = dbcontext;
        this.httpContextAccessor = httpContextAccessor;
        this.configuration = configuration;
    }

    private string CreateToken(User user, Profile profile)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, profile.Email),
            new Claim(ClaimTypes.Role, profile.ProfileType.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Secret").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: creds
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }


    private async Task<RefreshToken> GenerateRefreshToken(User user)
    {
        var currentDateTime = DateTime.UtcNow;

        RefreshToken refreshToken = new RefreshToken()
        {
            User = user,
            CreationTime = currentDateTime,
            UpdateTime = currentDateTime
        };

        await dbcontext.RefreshTokens.AddAsync(refreshToken);
        await dbcontext.SaveChangesAsync(true);

        return refreshToken;
    }

    public async Task<LoginResponseDto> Register(RegistrationDto registration)
    {
        if ((await dbcontext.Users.FirstOrDefaultAsync(u => u.Username == registration.Username)) is User)
        {
            throw new InvalidOperationException($"Profile with username {registration.Username} already exists.");
        }

        if ((await dbcontext.Profiles.FirstOrDefaultAsync(p => p.Email == registration.Email)) is Profile)
        {
            throw new InvalidOperationException($"Profile with email {registration.Email} already exists.");
        }

        var currentDateTime = DateTime.UtcNow;
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registration.Password);

        User user = new User()
        {
            Username = registration.Username,
            Password = hashedPassword,
            CreationTime = currentDateTime,
            UpdateTime = currentDateTime
        };

        Profile profile = new Profile()
        {
            Firstname = registration.FirstName,
            Lastname = registration.Lastname,
            Email = registration.Email,
            User = user,
            CreationTime = currentDateTime,
            UpdateTime = currentDateTime
        };

        await dbcontext.Users.AddAsync(user);
        await dbcontext.Profiles.AddAsync(profile);
        await dbcontext.SaveChangesAsync();

        var refreshToken = await GenerateRefreshToken(user);
        httpContextAccessor.HttpContext!.Response.Cookies.Append(
            "refresh-token",
            refreshToken.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            }
        );

        return new LoginResponseDto()
        {
            Firstname = profile.Firstname,
            Lastname = profile.Lastname,
            Email = profile.Email,
            ProfileType = profile.ProfileType,
            JwtToken = CreateToken(user, profile)
        };
    }

    public async Task<LoginResponseDto> Login(LoginCredentialsDto loginCredentials)
    {
        var user = await dbcontext.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Username == loginCredentials.Username);

        if (user is not null)
        {
            if (BCrypt.Net.BCrypt.Verify(loginCredentials.Password, user.Password))
            {
                var profile = user.Profile;

                var refreshToken = await GenerateRefreshToken(user);
                httpContextAccessor.HttpContext!.Response.Cookies.Append(
                    "refresh-token",
                    refreshToken.Token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    }
                );

                return new LoginResponseDto()
                {
                    Firstname = profile.Firstname,
                    Lastname = profile.Lastname,
                    Email = profile.Email,
                    ProfileType = profile.ProfileType,
                    JwtToken = CreateToken(user, profile)
                };
            }
            else
            {
                throw new InvalidOperationException("Invalid credentials");
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid credentials");
        }
    }

    public async Task<LoginResponseDto> RefreshToken()
    {
        var refreshKey = httpContextAccessor.HttpContext!.Request.Cookies["refresh-token"];

        var userProfileRefreshToken = await dbcontext.Users
            .Join(dbcontext.Profiles, u => u.Id, p => p.UserId, (user, profile) => new { user, profile })
            .Join(dbcontext.RefreshTokens, up => up.user.Id, rt => rt.UserId, (up, refreshToken) => new { up.user, up.profile, refreshToken })
            .FirstOrDefaultAsync(upr => upr.refreshToken.Token == refreshKey);

        if (userProfileRefreshToken is not null)
        {
            var user = userProfileRefreshToken.user;
            var profile = userProfileRefreshToken.profile;
            var oldRefreshToken = userProfileRefreshToken.refreshToken;

            if (oldRefreshToken.Active)
            {
                var timeToExpire = oldRefreshToken.ExpiryDate - DateTime.UtcNow;

                if (timeToExpire >= TimeSpan.Zero)
                {
                    if (timeToExpire.TotalDays < 1)
                    {
                        var newRefreshToken = await GenerateRefreshToken(user);
                        httpContextAccessor.HttpContext!.Response.Cookies.Append(
                            "refresh-token",
                            newRefreshToken.Token,
                            new CookieOptions
                            {
                                HttpOnly = true,
                                Expires = DateTimeOffset.UtcNow.AddDays(7)
                            }
                        );
                    }

                    return new LoginResponseDto()
                    {
                        Firstname = profile.Firstname,
                        Lastname = profile.Lastname,
                        Email = profile.Email,
                        ProfileType = profile.ProfileType,
                        JwtToken = CreateToken(user, profile)
                    };
                }
                else
                {
                    throw new InvalidOperationException("Refresh token is not valid");
                }
            }
            else
            {
                throw new InvalidOperationException("Refresh token is not valid");
            }
        }
        else
        {
            throw new InvalidOperationException("Refresh token is not valid");
        }
    }

    public async Task Logout()
    {
        var refreshKey = httpContextAccessor.HttpContext.Request.Cookies["refresh-token"];
        var refreshToken = await dbcontext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshKey);

        if (refreshToken is not null)
        {
            refreshToken.Active = false;
            await dbcontext.SaveChangesAsync();
        }
    }
}
