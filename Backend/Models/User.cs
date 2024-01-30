using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Users_T")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public Profile Profile { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }

    public DateTime CreationTime { get; set; }
    public DateTime UpdateTime { get; set; }
}
