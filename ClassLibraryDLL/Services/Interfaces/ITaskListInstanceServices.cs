using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskListInstanceServices
    {
        Task<IEnumerable<TaskListInstanceDTO>> GetAllTaskListInstancesAsync();
        Task<TaskListInstanceDTO> AddTaskListInstanceAsync(AddTaskListInstanceDTO addTaskListInstanceDTO);
        Task<TaskListInstanceDTO> UpdateTaskListInstanceAsync(int id, AddTaskListInstanceDTO addTaskListInstanceDTO);
        Task<TaskListInstanceDTO> GetTaskListInstanceByIDAsync(int id);
        Task<bool> DeleteTaskListInstanceByIDAsync(int id);
    }
}
