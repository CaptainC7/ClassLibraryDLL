using ClassLibraryDLL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskListTemplateHistoryServices
    {
        Task<TaskListTemplateHistoryDTO> AddHistoryRecord(TaskListTemplateHistoryDTO historyDTO);
        Task<IEnumerable<TaskListTemplateHistoryDTO>> GetAllHistoryRecords();
    }
}
