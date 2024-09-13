using ClassLibraryDLL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskGroupHistoryServices
    {
        Task AddTaskGroupHistoryAsync(TaskGroupHistoryDTO taskGroupHistoryDTO);
        Task<IEnumerable<TaskGroupHistoryDTO>> GetTaskGroupHistory();
    }
}
