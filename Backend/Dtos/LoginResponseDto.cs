using Backend.Models;

namespace Backend.Dtos;

public class LoginResponseDto
{
    public int ProfileId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public ProfileTypes ProfileType { get; set; }
    public string JwtToken { get; set; }
}
