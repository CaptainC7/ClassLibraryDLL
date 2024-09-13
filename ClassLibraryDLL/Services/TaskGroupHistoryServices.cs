using ClassLibraryDLL.Models;
using ClassLibraryDLL.Models.ApplicationDBContext;
using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ClassLibraryDLL.Services
{
    public class TaskGroupHistoryServices : ITaskGroupHistoryServices
    {
        private readonly ApplicationDBContext _dbContext;
        public TaskGroupHistoryServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task AddTaskGroupHistoryAsync(TaskGroupHistoryDTO taskGroupHistoryDTO)
        {
            var taskGroupHistory = new TaskGroupHistory
            {
                TaskGroupID = taskGroupHistoryDTO.TaskGroupID,
                GroupName = taskGroupHistoryDTO.GroupName,
                TaskListTemplateID = taskGroupHistoryDTO.TaskListTemplateID,
                GroupOrder = taskGroupHistoryDTO.GroupOrder,
                ChangedBy = taskGroupHistoryDTO.ChangedBy,
                ChangedDate = taskGroupHistoryDTO.ChangedDate,
                ChangedType = taskGroupHistoryDTO.ChangedType
            };

            _dbContext.TaskGroupHistory.Add(taskGroupHistory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskGroupHistoryDTO>> GetTaskGroupHistory()
        {
            var histories = await _dbContext.TaskGroupHistory
            .Include(h => h.ChangedByPerson)
            .Select(h => new TaskGroupHistoryDTO
            {
                ID = h.ID,
                TaskGroupID = h.TaskGroupID,
                GroupName = h.GroupName,
                TaskListTemplateID = h.TaskListTemplateID,
                GroupOrder = h.GroupOrder,
                ChangedBy = h.ChangedBy,
                ChangedByPerson = new PersonDTO
                {
                    FName = h.ChangedByPerson.FName,
                    LName = h.ChangedByPerson.LName,
                    Gender = h.ChangedByPerson.Gender,
                    BDate = h.ChangedByPerson.BDate,
                    Username = h.ChangedByPerson.Username
                },
                ChangedDate = h.ChangedDate,
                ChangedType = h.ChangedType
            })
            .ToListAsync();

            return histories;
        }
    }
}
