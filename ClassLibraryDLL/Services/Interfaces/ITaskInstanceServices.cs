using ClassLibraryDLL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskInstanceServices
    {
        Task<IEnumerable<TaskInstanceDTO>> GetTaskInstancebyTaskGroupInstanceID(int taskGroupInstanceID);
        //    Task<TaskInstanceDTO> AddTaskInstance(AddTaskInstanceDTO addTaskInstanceDTO);
        //    Task<TaskInstanceDTO> UpdateTaskInstance(int id, AddTaskInstanceDTO addTaskInstanceDTO);
        //    Task<bool> DeleteTaskInstance(int id);
    }
}
