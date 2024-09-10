using ClassLibraryDLL.Models.ApplicationDBContext;
using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Models;
using ClassLibraryDLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ClassLibraryDLL.Services
{
    public class TaskServices : ITaskServices
    {
        private readonly ApplicationDBContext _dbContext;
        public TaskServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksByTaskGroupIdAsync(int taskGroupId)
        {
            var tasks = await _dbContext.Task
                .Where(task => task.TaskGroupID == taskGroupId)
                .Include(task => task.TaskGroup)
                .Select(task => new TaskDTO
                {
                    ID = task.ID,
                    TaskName = task.TaskName,
                    Description = task.Description,
                    TaskGroupID = task.TaskGroupID,
                    TaskOrder = task.TaskOrder,
                    DependancyTaskID = task.DependancyTaskID,

                    TaskGroup = new TaskGroupDTO
                    {
                        Id = task.TaskGroup.Id,
                        GroupName = task.TaskGroup.GroupName,
                        TaskListTemplateID = task.TaskGroup.TaskListTemplateID,
                        GroupOrder = task.TaskGroup.GroupOrder
                    }
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskDTO> AddTaskAsync(AddTaskDTO addTaskDTO)
        {
            var maxTaskOrder = await _dbContext.Task
            .Where(task => task.TaskGroupID == addTaskDTO.TaskGroupID)
            .MaxAsync(task => (int?)task.TaskOrder) ?? 0;

            var task = new Models.Task
            {
                TaskName = addTaskDTO.TaskName,
                Description = addTaskDTO.Description,
                TaskGroupID = addTaskDTO.TaskGroupID,
                DependancyTaskID = addTaskDTO.DependancyTaskID > 0 ? addTaskDTO.DependancyTaskID : (int?)null,
                TaskOrder = maxTaskOrder + 1
            };

            _dbContext.Task.Add(task);
            await _dbContext.SaveChangesAsync();

            var newTaskDTO = new TaskDTO
            {
                ID = task.ID,
                TaskName = task.TaskName,
                Description = task.Description,
                TaskGroupID = task.TaskGroupID,
                TaskOrder = task.TaskOrder,
                DependancyTaskID = task.DependancyTaskID,
            };

            return newTaskDTO;
        }

        public async Task<TaskDTO> UpdateTaskAsync(int id, AddTaskDTO addTaskDTO)
        {
            var task = await _dbContext.Task.FindAsync(id);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            task.TaskName = addTaskDTO.TaskName;
            task.Description = addTaskDTO.Description;
            task.TaskGroupID = addTaskDTO.TaskGroupID;
            task.DependancyTaskID = addTaskDTO.DependancyTaskID > 0 ? addTaskDTO.DependancyTaskID : (int?)null;

            _dbContext.Task.Update(task);
            await _dbContext.SaveChangesAsync();

            return new TaskDTO
            {
                ID = task.ID,
                TaskName = task.TaskName,
                Description = task.Description,
                TaskGroupID = task.TaskGroupID,
                DependancyTaskID= task.DependancyTaskID,
            };
        }

        //public async Task<bool> DeleteTaskByID(int id)
        //{
        //    var task = await _dbContext.Task.FindAsync(id);
        //    if (task == null)
        //    {
        //        return false;
        //    }

        //    _dbContext.Task.Remove(task);
        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _dbContext.Task.FindAsync(id);
            if (task == null)
            {
                return false; // Task not found
            }

            _dbContext.Task.Remove(task);
            await _dbContext.SaveChangesAsync();

            // Reorder tasks in the same group
            await ReorderTasksAsync(task.TaskGroupID);

            return true;
        }

        private async System.Threading.Tasks.Task ReorderTasksAsync(int taskGroupId)
        {
            var tasks = await _dbContext.Task
                .Where(t => t.TaskGroupID == taskGroupId)
                .OrderBy(t => t.TaskOrder)
                .ToListAsync();

            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].TaskOrder = i + 1;
            }

            _dbContext.Task.UpdateRange(tasks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
