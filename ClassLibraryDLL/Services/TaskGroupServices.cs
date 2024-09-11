using ClassLibraryDLL.Models.ApplicationDBContext;
using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryDLL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ClassLibraryDLL.Services
{
    public class TaskGroupServices : ITaskGroupServices
    {
        private readonly ApplicationDBContext _dbContext;
        public TaskGroupServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<TaskGroupDTO>> GetTaskGroupsByTemplateIdAsync(int taskListTemplateId)
        {
            var taskGroups = await _dbContext.TaskGroup
                .Where(taskGroup => taskGroup.TaskListTemplateID == taskListTemplateId)
                .Include(taskGroup => taskGroup.TaskListTemplate)
                .Select(TaskGroup => new TaskGroupDTO
                {
                    Id = TaskGroup.Id,
                    GroupName = TaskGroup.GroupName,
                    TaskListTemplateID = TaskGroup.TaskListTemplateID,
                    GroupOrder = TaskGroup.GroupOrder,

                    TaskListTemplate = new TaskListTemplateDTO
                    {
                        ID = taskListTemplateId,
                        TempName = TaskGroup.TaskListTemplate.TempName,
                        CreatedBy = TaskGroup.TaskListTemplate.CreatedBy,
                        CreatedDate = TaskGroup.TaskListTemplate.CreatedDate,
                    }
                })
                .ToListAsync();

            return taskGroups;
        }

        public async Task<TaskGroupDTO> AddTaskGroupAsync(AddTaskGroupDTO addTaskGroupDTO)
        {
            var maxGroupOrder = await _dbContext.TaskGroup
            .Where(tg => tg.TaskListTemplateID == addTaskGroupDTO.TaskListTemplateID)
            .MaxAsync(tg => (int?)tg.GroupOrder) ?? 0;

            var taskGroup = new TaskGroup
            {
                GroupName = addTaskGroupDTO.GroupName,
                TaskListTemplateID = addTaskGroupDTO.TaskListTemplateID,
                GroupOrder = maxGroupOrder + 1
            };

            _dbContext.TaskGroup.Add(taskGroup);
            await _dbContext.SaveChangesAsync();

            var newTaskGroupDTO = new TaskGroupDTO
            {
                Id=taskGroup.Id,
                GroupName = taskGroup.GroupName,
                TaskListTemplateID = taskGroup.TaskListTemplateID,
                GroupOrder = taskGroup.GroupOrder,
            };

            return newTaskGroupDTO;
        }

        public async Task<TaskGroupDTO> UpdateTaskGroupAsync(int id, AddTaskGroupDTO addTaskGroupDTO)
        {
            var taskGroup = await _dbContext.TaskGroup.FindAsync(id);

            if (taskGroup == null)
            {
                throw new KeyNotFoundException("TaskGroup not found");
            }

            taskGroup.GroupName = addTaskGroupDTO.GroupName;
            taskGroup.TaskListTemplateID = addTaskGroupDTO.TaskListTemplateID;

            _dbContext.TaskGroup.Update(taskGroup);
            await _dbContext.SaveChangesAsync();

            return new TaskGroupDTO
            {
                Id = taskGroup.Id,
                GroupName = taskGroup.GroupName,
                TaskListTemplateID = taskGroup.TaskListTemplateID,
                GroupOrder = taskGroup.GroupOrder
            };
        }

        //public async Task<bool> DeleteTaskGroupByID(int id)
        //{
        //    var taskGroup = await _dbContext.TaskGroup.FindAsync(id);
        //    if (taskGroup == null)
        //    {
        //        return false;
        //    }

        //    _dbContext.TaskGroup.Remove(taskGroup);
        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> DeleteTaskGroupAsync(int id)
        {
            var task = await _dbContext.TaskGroup.FindAsync(id);
            if (task == null)
            {
                return false; // Task not found
            }

            _dbContext.TaskGroup.Remove(task);
            await _dbContext.SaveChangesAsync();

            // Reorder tasks in the same group
            await ReorderTasksAsync(task.TaskListTemplateID);

            return true;
        }

        private async System.Threading.Tasks.Task ReorderTasksAsync(int taskListTemplateID)
        {
            var tasks = await _dbContext.TaskGroup
                .Where(t => t.TaskListTemplateID == taskListTemplateID)
                .OrderBy(t => t.GroupOrder)
                .ToListAsync();

            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].GroupOrder = i + 1;
            }

            _dbContext.TaskGroup.UpdateRange(tasks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
