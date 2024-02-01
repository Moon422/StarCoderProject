using System.ComponentModel.DataAnnotations;
using Backend.Tests.Validators;

namespace Backend.Tests.Dtos;

public class RegistrationDto
{
    [Required]
    [MaxLength(256)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(256)]
    public string Lastname { get; set; }

    // [Required]
    // public DateTime DateOfBirth { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(256)]
    public string Username { get; set; }

    [Required]
    [PasswordValidator]
    public string Password { get; set; }
}
