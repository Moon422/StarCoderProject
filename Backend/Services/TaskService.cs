using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public interface ITaskService
{
    Task<ReadTaskDto> CreateTask(CreateTaskDto createTaskDto);
    Task<List<ReadTaskDto>> GetAllTasks(bool adminAccess = false);
    Task<ReadTaskDto> UpdateTask(int taskId, UpdateTaskDto updateTaskDto, bool adminAccess = false);
    Task DeleteTask(int taskId, bool adminAccess = false);
}

public class TaskService : ITaskService
{
    private readonly StarDb dbcontext;
    private readonly IHttpContextAccessor httpContextAccessor;

    public TaskService(StarDb dbcontext, IHttpContextAccessor httpContextAccessor)
    {
        this.dbcontext = dbcontext;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ReadTaskDto> CreateTask(CreateTaskDto createTaskDto)
    {
        var email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        var profile = await dbcontext.Profiles.FirstOrDefaultAsync(p => p.Email == email);

        TaskModel task = new TaskModel()
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            Profile = profile,
            CreationTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow
        };

        await dbcontext.Tasks.AddAsync(task);
        await dbcontext.SaveChangesAsync();

        return new ReadTaskDto()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            TaskStatus = task.TaskStatus,
            ProfileId = profile.Id
        };
    }

    public async Task<List<ReadTaskDto>> GetAllTasks(bool adminAccess = false)
    {
        if (adminAccess)
        {
            return await dbcontext.Tasks.Select(t => t.ToReadDto()).ToListAsync();
        }
        else
        {
            var email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var profile = await dbcontext.Profiles.FirstOrDefaultAsync(p => p.Email == email);

            return await dbcontext.Tasks.Where(t => t.Profile == profile).Select(t => t.ToReadDto()).ToListAsync();
        }
    }

    public async Task DeleteTask(int taskId, bool adminAccess = false)
    {
        TaskModel task = null;

        if (adminAccess)
        {
            task = await dbcontext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }
        else
        {
            var email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var profile = await dbcontext.Profiles.FirstOrDefaultAsync(p => p.Email == email);
            task = await dbcontext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.Profile == profile);
        }

        if (task is not null)
        {
            dbcontext.Tasks.Remove(task);
            await dbcontext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException($"Task with id {taskId} not found");
        }
    }

    public async Task<ReadTaskDto> UpdateTask(int taskId, UpdateTaskDto updateTaskDto, bool adminAccess = false)
    {
        TaskModel task = null;

        if (adminAccess)
        {
            task = await dbcontext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }
        else
        {
            var email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var profile = await dbcontext.Profiles.FirstOrDefaultAsync(p => p.Email == email);
            task = await dbcontext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.Profile == profile);
        }

        if (task is not null)
        {
            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.TaskStatus = updateTaskDto.TaskStatus;
            await dbcontext.SaveChangesAsync();

            return task.ToReadDto();
        }
        else
        {
            throw new InvalidOperationException($"Task with id {taskId} not found");
        }
    }
}
