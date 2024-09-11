using ClassLibraryDLL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskGroupInstanceServices
    {
        Task<IEnumerable<TaskGroupInstanceDTO>> GetTaskGroupInstancesByTaskListInstanceID(int taskListInstanceID);
        Task<TaskGroupInstanceDTO> AddTaskGroupInstance(AddTaskGroupInstanceDTO addTaskGroupInstanceDTO);
        Task<TaskGroupInstanceDTO> UpdateTaskGroupInstance(int id, AddTaskGroupInstanceDTO addTaskGroupInstanceDTO);
        Task<bool> DeleteTaskGroupInstance(int id);
    }
}
