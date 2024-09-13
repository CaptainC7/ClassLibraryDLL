using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Models;
using ClassLibraryDLL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using ClassLibraryDLL.Models.ApplicationDBContext;

namespace ClassLibraryDLL.Services
{
    public class TaskHistoryServices : ITaskHistoryServices
    {
        private readonly ApplicationDBContext _dbContext;

        public TaskHistoryServices(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddTaskHistoryAsync(TaskHistoryDTO taskHistoryDTO)
        {
            var taskHistory = new TaskHistory
            {
                TaskID = taskHistoryDTO.TaskID,
                TaskName = taskHistoryDTO.TaskName,
                Description = taskHistoryDTO.Description,
                TaskGroupID = taskHistoryDTO.TaskGroupID,
                TaskOrder = taskHistoryDTO.TaskOrder,
                DependancyTaskID = taskHistoryDTO.DependancyTaskID,
                ChangedBy = taskHistoryDTO.ChangedBy,
                ChangedDate = taskHistoryDTO.ChangedDate,
                ChangedType = taskHistoryDTO.ChangedType
            };

            _dbContext.TaskHistory.Add(taskHistory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskHistoryDTO>> GetAllTaskHistoriesAsync()
        {
            var taskHistories = await _dbContext.TaskHistory
                .Include(th => th.TaskGroup)
                .Include(th => th.DependancyTask)
                .Include(th => th.ChangedByPerson)
                .ToListAsync();

            var taskHistoryDTOs = taskHistories.Select(th => new TaskHistoryDTO
            {
                ID = th.ID,
                TaskID = th.TaskID,
                TaskName = th.TaskName,
                Description = th.Description,
                TaskGroupID = th.TaskGroupID,
                TaskOrder = th.TaskOrder,
                DependancyTaskID = th.DependancyTaskID,
                ChangedBy = th.ChangedBy,
                ChangedDate = th.ChangedDate,
                ChangedType = th.ChangedType,
                //TaskGroup = new TaskGroupDTO
                TaskGroup = th.TaskGroup != null ? new TaskGroupDTO
                {
                    Id = th.TaskGroup.Id,
                    GroupName = th.TaskGroup.GroupName,
                    TaskListTemplateID = th.TaskGroup.TaskListTemplateID,
                    GroupOrder = th.TaskGroup.GroupOrder
                } : null,
                //DependancyTask = new TaskDTO
                DependancyTask = th.DependancyTask != null ? new TaskDTO
                {
                    ID = th.DependancyTask.ID,
                    TaskName = th.DependancyTask.TaskName,
                    Description = th.DependancyTask.Description,
                    TaskGroupID = th.DependancyTask.TaskGroupID,
                    TaskOrder = th.DependancyTask.TaskOrder,
                    DependancyTaskID = th.DependancyTask.DependancyTaskID
                } : null,
                //ChangedByPerson = new PersonDTO
                ChangedByPerson = th.ChangedByPerson != null ? new PersonDTO
                {
                    FName = th.ChangedByPerson.FName,
                    LName = th.ChangedByPerson.LName,
                    Gender = th.ChangedByPerson.Gender,
                    BDate = th.ChangedByPerson.BDate,
                    Username = th.ChangedByPerson.Username
                } : null
            }).ToList();

            return taskHistoryDTOs;
        }
    }
}
