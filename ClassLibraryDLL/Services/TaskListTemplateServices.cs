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

namespace ClassLibraryDLL.Services
{
    public class TaskListTemplateServices : ITaskListTemplateServices
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ITaskListTemplateHistoryServices _historyService;
        public TaskListTemplateServices(ApplicationDBContext dBContext, ITaskListTemplateHistoryServices historyService)
        {
            _dbContext = dBContext;
            _historyService = historyService;
        }

        public async Task<IEnumerable<TaskListTemplateDTO>> GetAllTaskListTemplatesAsync()
        {
            var taskListTemplate = await _dbContext.TaskListTemplate
                .Include(t => t.CreatedByPerson)
                .Select(t => new TaskListTemplateDTO
                {
                    ID = t.ID,
                    TempName = t.TempName,
                    CreatedDate = t.CreatedDate,
                    CreatedBy = t.CreatedBy,

                    CreatedByPerson = new PersonDTO
                    {
                        FName = t.CreatedByPerson.FName,
                        LName = t.CreatedByPerson.LName,
                        Gender = t.CreatedByPerson.Gender,
                        BDate = t.CreatedByPerson.BDate,
                        Username = t.CreatedByPerson.Username,
                        Password = t.CreatedByPerson.Password
                    }
                })
                .ToListAsync();

            return taskListTemplate;
        }

        public async Task<TaskListTemplateDTO> AddTemplateAsync(AddTaskListTemplateDTO addTaskListTemplateDTO)
        {
            var template = new TaskListTemplate
            {
                TempName = addTaskListTemplateDTO.TempName,
                CreatedDate = DateTime.Now,
                CreatedBy = addTaskListTemplateDTO.CreatedBy,
            };

            _dbContext.TaskListTemplate.Add(template);
            await _dbContext.SaveChangesAsync();

            return new TaskListTemplateDTO
            {
                ID = template.ID,
                TempName = template.TempName,
                CreatedBy = template.CreatedBy,
                CreatedDate = template.CreatedDate,
            };
        }

        public async Task<TaskListTemplateDTO> UpdateTemplate(int id, UpdateTaskListTemplateDTO updateTaskListTemplateDTO, int userID)
        {
            var template = await _dbContext.TaskListTemplate.FindAsync(id);

            if (template == null)
            {
                return null;
            }

            var historyDTO = new TaskListTemplateHistoryDTO
            {
                TaskListTemplateID = template.ID,
                TempName = template.TempName,
                CreatedBy = template.CreatedBy,
                CreatedDate = template.CreatedDate,
                ChangedBy = userID,
                ChangedDate = DateTime.Now,
                ChangedType = "Update"
            };
            await _historyService.AddHistoryRecord(historyDTO);

            template.TempName = updateTaskListTemplateDTO.TempName;

            await _dbContext.SaveChangesAsync();

            return new TaskListTemplateDTO
            {
                ID = template.ID,
                TempName = template.TempName,
                CreatedBy = template.CreatedBy,
                CreatedDate = template.CreatedDate,
            };
        }

        public async Task<TaskListTemplateDTO> GetTemplateByIDAsync(int ID)
        {
            var template = await _dbContext.TaskListTemplate
            .Include(t => t.CreatedByPerson)
            .FirstOrDefaultAsync(t => t.ID == ID);

            if (template == null)
            {
                return null;
            }

            return new TaskListTemplateDTO
            {
                ID = template.ID,
                TempName = template.TempName,
                CreatedBy = template.CreatedBy,
                CreatedDate = template.CreatedDate,
                CreatedByPerson = template.CreatedByPerson != null
                    ? new PersonDTO
                    {
                        FName = template.CreatedByPerson.FName,
                        LName = template.CreatedByPerson.LName,
                        Gender = template.CreatedByPerson.Gender,
                        BDate = template.CreatedByPerson.BDate,
                        Username = template.CreatedByPerson.Username,
                        Password = template.CreatedByPerson.Password,
                    }
                    : null
            };
        }

        public async Task<bool> DeleteTemplateByID(int id, int userID)
        {
            var template = await _dbContext.TaskListTemplate.FindAsync(id);
            if (template == null)
            {
                return false;
            }

            var historyDTO = new TaskListTemplateHistoryDTO
            {
                TaskListTemplateID = template.ID,
                TempName = template.TempName,
                CreatedBy = template.CreatedBy,
                CreatedDate = template.CreatedDate,
                ChangedBy = userID,
                ChangedDate = DateTime.Now,
                ChangedType = "Delete"
            };
            await _historyService.AddHistoryRecord(historyDTO);

            _dbContext.TaskListTemplate.Remove(template);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
