using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos;

public abstract class TaskDto
{
    [Required]
    [MaxLength(255)]
    [MinLength(1)]
    public string Title { get; set; }

    [Required]
    [MaxLength(1024)]
    [MinLength(1)]
    public string Description { get; set; }


}

public class ReadTaskDto : TaskDto
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public TaskStatus TaskStatus { get; set; }
}

public class CreateTaskDto : TaskDto
{
    [Required]
    public int ProfileId { get; set; }
}

public class UpdateTaskDto : TaskDto
{
    [Required]
    public TaskStatus TaskStatus { get; set; }
}
