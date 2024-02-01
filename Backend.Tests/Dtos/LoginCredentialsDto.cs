using System.ComponentModel.DataAnnotations;
using Backend.Tests.Validators;

namespace Backend.Tests.Dtos;

public class LoginCredentialsDto
{
    [Required]
    [MaxLength(256)]
    public string Username { get; set; }

    [Required]
    [PasswordValidator]
    public string Password { get; set; }
}
