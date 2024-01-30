using System;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.Migrations;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    ITaskService taskService;

    public TaskController(ITaskService taskService)
    {
        this.taskService = taskService;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            var taskDto = await taskService.CreateTask(createTaskDto);
            return Ok(taskDto);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetAllTasks() // pagination to be added later
    {
        try
        {
            var taskDtos = await taskService.GetAllTasks();
            return Ok(taskDtos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }

    [HttpDelete("{taskId}"), Authorize]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        try
        {
            await taskService.DeleteTask(taskId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }

    [HttpPut("{taskId}"), Authorize]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var taskDto = await taskService.UpdateTask(taskId, updateTaskDto);
            return Ok(taskDto);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }
}
