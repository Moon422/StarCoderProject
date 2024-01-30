using System;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto registration)
    {
        try
        {
            var loginResponse = await authService.Register(registration);
            return Ok(loginResponse);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentialsDto loginCredentialsDto)
    {
        try
        {
            var loginResponse = await authService.Login(loginCredentialsDto);
            return Ok(loginResponse);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }

    [HttpGet("[controller]/refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var loginResponse = await authService.RefreshToken();
            return Ok(loginResponse);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }

    [HttpGet("[controller]/logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await authService.Logout();
            return NoContent();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }
}
