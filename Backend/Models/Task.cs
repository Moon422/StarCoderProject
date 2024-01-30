using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Dtos;

namespace Backend.Models;

[Table("Tasks_T")]
public class TaskModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    [MaxLength(1024)]
    public string Description { get; set; }

    [Required]
    public TaskStatus TaskStatus { get; set; } = TaskStatus.INCOMPLETE;

    public int ProfileId { get; set; }
    public Profile Profile { get; set; }

    public DateTime CreationTime { get; set; }
    public DateTime UpdateTime { get; set; }

    public ReadTaskDto ToReadDto()
    {
        return new ReadTaskDto()
        {
            Id = Id,
            Title = Title,
            Description = Description,
            TaskStatus = TaskStatus,
            ProfileId = ProfileId
        };
    }
}
