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
        private readonly ITaskHistoryServices _historyService;
        public TaskServices(ApplicationDBContext dBContext, ITaskHistoryServices historyService)
        {
            _dbContext = dBContext;
            _historyService = historyService;
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

        public async Task<TaskDTO> AddTaskAsync(AddTaskDTO addTaskDTO, int userID)
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

            var taskHistoryDTO = new TaskHistoryDTO
            {
                TaskID = task.ID,
                TaskName = task.TaskName,
                Description = task.Description,
                TaskGroupID = task.TaskGroupID,
                TaskOrder = task.TaskOrder,
                DependancyTaskID = task.DependancyTaskID,
                ChangedBy = userID,
                ChangedDate = DateTime.Now,
                ChangedType = "Create"
            };

            await _historyService.AddTaskHistoryAsync(taskHistoryDTO);

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

        public async Task<TaskDTO> UpdateTaskAsync(int id, AddTaskDTO addTaskDTO, int userID)
        {
            var task = await _dbContext.Task.FindAsync(id);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            var taskHistoryDTO = new TaskHistoryDTO
            {
                TaskID = task.ID,
                TaskName = task.TaskName,
                Description = task.Description,
                TaskGroupID = task.TaskGroupID,
                TaskOrder = task.TaskOrder,
                DependancyTaskID = task.DependancyTaskID,
                ChangedBy = userID,
                ChangedDate = DateTime.Now,
                ChangedType = "Update"
            };

            await _historyService.AddTaskHistoryAsync(taskHistoryDTO);

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

        public async Task<bool> DeleteTaskAsync(int id, int userID)
        {
            var task = await _dbContext.Task.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            var taskHistoryDTO = new TaskHistoryDTO
            {
                TaskID = task.ID,
                TaskName = task.TaskName,
                Description = task.Description,
                TaskGroupID = task.TaskGroupID,
                TaskOrder = task.TaskOrder,
                DependancyTaskID = task.DependancyTaskID,
                ChangedBy = userID,
                ChangedDate = DateTime.Now,
                ChangedType = "Delete"
            };

            await _historyService.AddTaskHistoryAsync(taskHistoryDTO);

            _dbContext.Task.Remove(task);
            await _dbContext.SaveChangesAsync();

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
