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
    public class TaskListTemplateHistoryServices : ITaskListTemplateHistoryServices
    {
        private readonly ApplicationDBContext _dbContext;

        public TaskListTemplateHistoryServices(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskListTemplateHistoryDTO> AddHistoryRecord(TaskListTemplateHistoryDTO historyDTO)
        {
            var history = new TaskListTemplateHistory
            {
                TaskListTemplateID = historyDTO.TaskListTemplateID,
                TempName = historyDTO.TempName,
                CreatedBy = historyDTO.CreatedBy,
                CreatedDate = historyDTO.CreatedDate,
                ChangedBy = historyDTO.ChangedBy,
                ChangedDate = historyDTO.ChangedDate,
                ChangedType = historyDTO.ChangedType
            };

            _dbContext.TaskListTemplateHistory.Add(history);
            await _dbContext.SaveChangesAsync();
            return historyDTO;
        }

        public async Task<IEnumerable<TaskListTemplateHistoryDTO>> GetAllHistoryRecords()
        {
            var histories = await _dbContext.TaskListTemplateHistory
            .Include(h => h.CreatedByPerson)
            .Include(h => h.ChangedByPerson)
            .Select(h => new TaskListTemplateHistoryDTO
            {
                ID = h.ID,
                TaskListTemplateID = h.TaskListTemplateID,
                TempName = h.TempName,
                CreatedBy = h.CreatedBy,
                CreatedByPerson = new PersonDTO
                {
                    FName = h.CreatedByPerson.FName,
                    LName = h.CreatedByPerson.LName,
                    Gender = h.CreatedByPerson.Gender,
                    BDate = h.CreatedByPerson.BDate,
                    Username = h.CreatedByPerson.Username
                },
                CreatedDate = h.CreatedDate,
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
