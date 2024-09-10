using ClassLibraryDLL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskServices
    {
        Task<IEnumerable<TaskDTO>> GetTasksByTaskGroupIdAsync(int taskGroupID);
        Task<TaskDTO> AddTaskAsync(AddTaskDTO addTaskDTO);
        Task<TaskDTO> UpdateTaskAsync(int id, AddTaskDTO addTaskDTO);
        Task<bool> DeleteTaskAsync(int id);
    }
}
