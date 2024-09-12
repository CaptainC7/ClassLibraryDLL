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
        Task<IEnumerable<TaskListTemplateDTO>> GetAllTaskListTemplatesAsync();
        Task<TaskListTemplateDTO> AddTemplateAsync(AddTaskListTemplateDTO addTaskListTemplateDTO);
        Task<TaskListTemplateDTO> UpdateTemplate(int id, UpdateTaskListTemplateDTO updateTaskListTemplateDTO, int userID);
        Task<TaskListTemplateDTO> GetTemplateByIDAsync(int ID);
        Task<bool> DeleteTemplateByID(int id, int userID);
    }
}
