using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskGroupServices
    {
        Task<IEnumerable<TaskGroupDTO>> GetTaskGroupsByTemplateIdAsync(int taskListTemplateId);
        //Task<TaskGroupDTO> AddTaskGroup(TaskGroup taskGroup);
        Task<TaskGroupDTO> AddTaskGroupAsync(AddTaskGroupDTO addTaskGroupDTO);
        Task<TaskGroupDTO> UpdateTaskGroupAsync(int id, AddTaskGroupDTO addTaskGroupDTO);
        //Task<TaskListTemplate> GetTemplateByID(int id);
        Task<bool> DeleteTaskGroupByID(int id);
    }

}
