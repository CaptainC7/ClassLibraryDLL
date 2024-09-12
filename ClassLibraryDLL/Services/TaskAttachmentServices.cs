using ClassLibraryDLL.Models;
using ClassLibraryDLL.Models.ApplicationDBContext;
using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services
{
    public class TaskAttachmentServices : ITaskAttachmentServices
    {
        private readonly ApplicationDBContext _dbContext;
        public TaskAttachmentServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<TaskAttachmentDTO>> GetAllTaskAttachmentsByTaskInstanceId(int taskInstanceId)
        {
            return await _dbContext.TaskAttachment
            .Where(ta => ta.TaskInstanceID == taskInstanceId)
            .Include(ta => ta.UploadedByPerson)
            .Select(ta => new TaskAttachmentDTO
            {
                ID = ta.ID,
                TaskInstanceID = ta.TaskInstanceID,
                FilePath = ta.FilePath,
                UploadedBy = ta.UploadedBy,
                UploadedDate = ta.UploadedDate,
                UploadedByPerson = new PersonDTO
                {
                    FName = ta.UploadedByPerson.FName,
                    LName = ta.UploadedByPerson.LName,
                    Gender = ta.UploadedByPerson.Gender,
                    BDate = ta.UploadedByPerson.BDate,
                    Username = ta.UploadedByPerson.Username,
                    Password = ta.UploadedByPerson.Password,
                }
            }).ToListAsync();
        }

        public async Task<TaskAttachmentDTO> AddTaskAttachmentAsync(AddTaskAttachmentDTO addTaskAttachmentDTO)
        {
            var newAttachment = new TaskAttachment
            {
                TaskInstanceID = addTaskAttachmentDTO.TaskInstanceID,
                FilePath = addTaskAttachmentDTO.FilePath,
                UploadedBy = addTaskAttachmentDTO.UploadedBy,
                UploadedDate = addTaskAttachmentDTO.UploadedDate
            };

            _dbContext.TaskAttachment.Add(newAttachment);
            await _dbContext.SaveChangesAsync();

            return new TaskAttachmentDTO
            {
                ID = newAttachment.ID,
                TaskInstanceID = newAttachment.TaskInstanceID,
                FilePath = newAttachment.FilePath,
                UploadedBy = newAttachment.UploadedBy,
                UploadedDate = newAttachment.UploadedDate,
                UploadedByPerson = await _dbContext.Person
                    .Where(p => p.ID == newAttachment.UploadedBy)
                    .Select(p => new PersonDTO
                    {
                        FName = p.FName,
                        LName = p.LName,
                        Gender = p.Gender,
                        BDate = p.BDate,
                        Username = p.Username,
                        Password = p.Password,
                    }).FirstOrDefaultAsync()
            };
        }

        public async Task<TaskAttachmentDTO> UpdateTaskAttachmentAsync(int id, AddTaskAttachmentDTO addTaskAttachmentDTO)
        {
            var existingAttachment = await _dbContext.TaskAttachment
            .Include(ta => ta.UploadedByPerson)
            .FirstOrDefaultAsync(ta => ta.ID == id);

            if (existingAttachment == null)
            {
                return null;
            }

            existingAttachment.TaskInstanceID = addTaskAttachmentDTO.TaskInstanceID;
            existingAttachment.FilePath = addTaskAttachmentDTO.FilePath;
            existingAttachment.UploadedBy = addTaskAttachmentDTO.UploadedBy;
            existingAttachment.UploadedDate = addTaskAttachmentDTO.UploadedDate;

            await _dbContext.SaveChangesAsync();

            return new TaskAttachmentDTO
            {
                ID = existingAttachment.ID,
                TaskInstanceID = existingAttachment.TaskInstanceID,
                FilePath = existingAttachment.FilePath,
                UploadedBy = existingAttachment.UploadedBy,
                UploadedDate = existingAttachment.UploadedDate,
                UploadedByPerson = existingAttachment.UploadedByPerson != null ? new PersonDTO
                {
                    FName = existingAttachment.UploadedByPerson.FName,
                    LName = existingAttachment.UploadedByPerson.LName,
                    Gender = existingAttachment.UploadedByPerson.Gender,
                    BDate = existingAttachment.UploadedByPerson.BDate,
                    Username = existingAttachment.UploadedByPerson.Username,
                    Password = existingAttachment.UploadedByPerson.Password
                } : null
            };
        }

        public async Task<bool> DeleteTaskAttachment(int id)
        {
            var template = await _dbContext.TaskAttachment.FindAsync(id);
            if (template == null)
            {
                return false;
            }

            _dbContext.TaskAttachment.Remove(template);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
