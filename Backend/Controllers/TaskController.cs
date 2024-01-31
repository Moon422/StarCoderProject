using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.Migrations;
using Backend.Models;
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

    [HttpPost, Authorize(Roles = "USER")]
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

    [HttpGet, Authorize(Roles = "USER,ADMIN")]
    public async Task<IActionResult> GetAllTasks() // pagination to be added later
    {
        try
        {
            var role = HttpContext.User!.FindFirstValue(ClaimTypes.Role)!;
            var adminAccess = role.Equals(ProfileTypes.ADMIN.ToString());
            var taskDtos = await taskService.GetAllTasks(adminAccess);
            return Ok(taskDtos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please try againg");
        }
    }

    [HttpDelete("{taskId}"), Authorize(Roles = "USER,ADMIN")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        try
        {
            var role = HttpContext.User!.FindFirstValue(ClaimTypes.Role)!;
            var adminAccess = role.Equals(ProfileTypes.ADMIN.ToString());
            await taskService.DeleteTask(taskId, adminAccess);
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

    [HttpPut("{taskId}"), Authorize(Roles = "USER,ADMIN")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var role = HttpContext.User!.FindFirstValue(ClaimTypes.Role)!;
            var adminAccess = role.Equals(ProfileTypes.ADMIN.ToString());
            var taskDto = await taskService.UpdateTask(taskId, updateTaskDto, adminAccess);
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
