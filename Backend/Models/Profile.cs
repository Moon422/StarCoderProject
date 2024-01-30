using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Profiles_T")]
public class Profile
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Firstname { get; set; }

    [Required]
    [MaxLength(256)]
    public string Lastname { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    // [Required]
    // public DateTime DateOfBirth { get; set; }

    [Required]
    public ProfileTypes ProfileType { get; set; } = ProfileTypes.USER;

    public int UserId { get; set; }
    public User User { get; set; }

    public List<TaskModel> Tasks { get; set; }

    public DateTime CreationTime { get; set; }
    public DateTime UpdateTime { get; set; }
}
