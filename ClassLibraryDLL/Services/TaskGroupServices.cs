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
                .Select(TaskGroup => new TaskGroupDTO
                {
                    Id = TaskGroup.Id,
                    GroupName = TaskGroup.GroupName,
                    TaskListTemplateID = TaskGroup.TaskListTemplateID,
                    GroupOrder = TaskGroup.GroupOrder,
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
                GroupOrder = taskGroup.GroupOrder // Handle as per your trigger or logic
            };
        }

        //public async Task<TaskListTemplate> GetTemplateByID(int ID)
        //{
        //    var template = _dbContext.TaskListTemplate.Find(ID);
        //    if (template == null)
        //    {
        //        return null;
        //    }
        //    return template;
        //}

        public async Task<bool> DeleteTaskGroupByID(int id)
        {
            var taskGroup = await _dbContext.TaskGroup.FindAsync(id);
            if (taskGroup == null)
            {
                return false;
            }

            _dbContext.TaskGroup.Remove(taskGroup);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
