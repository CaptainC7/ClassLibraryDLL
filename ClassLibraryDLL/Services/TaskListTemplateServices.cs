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

        public async Task<IEnumerable<TaskListTemplate>> GetTemplates()
        {
            var templates = await _dbContext.TaskListTemplate.ToListAsync();
            return templates;
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
