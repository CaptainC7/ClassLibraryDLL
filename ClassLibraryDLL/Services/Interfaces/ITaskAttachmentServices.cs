using ClassLibraryDLL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface ITaskAttachmentServices
    {
        Task<IEnumerable<TaskAttachmentDTO>> GetAllTaskAttachmentsByTaskInstanceId(int taskInstanceId);
        Task<TaskAttachmentDTO> AddTaskAttachmentAsync(AddTaskAttachmentDTO addTaskAttachmentDTO);
        Task<TaskAttachmentDTO> UpdateTaskAttachmentAsync(int id, AddTaskAttachmentDTO addTaskAttachmentDTO);
        Task<bool> DeleteTaskAttachment(int id);
    }
}
