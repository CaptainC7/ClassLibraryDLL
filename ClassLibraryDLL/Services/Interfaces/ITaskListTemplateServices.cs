using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskListTemplateServices
    {
        Task<IEnumerable<TaskListTemplate>> GetTemplates();
        Task<TaskListTemplateDTO> AddTemplate(TaskListTemplateDTO taskListTemplate);
        Task<TaskListTemplateDTO> UpdateTemplate(int id, TaskListTemplateDTO taskListTemplateDTO);
        Task<TaskListTemplate> GetTemplateByID(int id);
        Task<bool> DeleteTemplateByID(int id);
    }
}
