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
        public TaskListTemplateServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        //public async Task<IEnumerable<TaskListTemplate>> GetTemplates()
        //{
        //    var templates = await _dbContext.TaskListTemplate.ToListAsync();
        //    return templates;
        //}

        public async Task<IEnumerable<TaskListTemplateDTO>> GetAllTaskListTemplatesAsync()
        {
            var taskListTemplate = await _dbContext.TaskListTemplate
                .Include(t => t.CreatedByPerson)
                .Select(t => new TaskListTemplateDTO
                {
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


        public async Task<TaskListTemplateDTO> AddTemplate(TaskListTemplateDTO taskListTemplateDTO)
        {
            var template = new TaskListTemplate
            {
                TempName = taskListTemplateDTO.TempName,
                CreatedBy = taskListTemplateDTO.CreatedBy,
                CreatedDate = taskListTemplateDTO.CreatedDate,
            };

            _dbContext.TaskListTemplate.Add(template);
            await _dbContext.SaveChangesAsync();
            return taskListTemplateDTO;
        }

        public async Task<TaskListTemplateDTO> UpdateTemplate(int id, TaskListTemplateDTO taskListTemplateDTO)
        {
            var template = _dbContext.TaskListTemplate.Find(id);

            if (template == null)
            {
                return null;
            }

            template.TempName = taskListTemplateDTO.TempName;
            template.CreatedBy = taskListTemplateDTO.CreatedBy;
            template.CreatedDate = taskListTemplateDTO.CreatedDate;

            await _dbContext.SaveChangesAsync();

            return taskListTemplateDTO;
        }

        public async Task<TaskListTemplate> GetTemplateByID(int ID)
        {
            var template = _dbContext.TaskListTemplate.Find(ID);
            if (template == null)
            {
                return null;
            }
            return template;
        }

        public async Task<bool> DeleteTemplateByID(int id)
        {
            var template = await _dbContext.TaskListTemplate.FindAsync(id);
            if (template == null)
            {
                return false;
            }

            _dbContext.TaskListTemplate.Remove(template);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
