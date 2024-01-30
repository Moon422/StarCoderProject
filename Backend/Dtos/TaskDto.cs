using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos;

public abstract class TaskDto
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    [MaxLength(1024)]
    public string Description { get; set; }

    [Required]
    public TaskStatus TaskStatus { get; set; }

    [Required]
    public int ProfileId { get; set; }
}

public class ReadTaskDto : TaskDto
{
    public int Id { get; set; }
}

public class CreateTaskDto : TaskDto { }
