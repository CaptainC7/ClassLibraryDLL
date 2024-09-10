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
        //Task<TaskListInstanceDTO> AddTaskListInstance(TaskListInstanceDTO taskListInstanceDTO);
        //Task<TaskListInstanceDTO> UpdateTaskListInstance(int id, TaskListInstanceDTO taskListInstanceDTO);
        //Task<TaskListInstance> GetTaskListInstanceByID(int id);
        //Task<bool> DeleteTaskListInstanceByID(int id);
    }
}
