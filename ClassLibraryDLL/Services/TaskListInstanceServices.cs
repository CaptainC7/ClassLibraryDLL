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
    public class TaskListInstanceServices : ITaskListInstanceServices
    {
        private readonly ApplicationDBContext _dbContext;
        public TaskListInstanceServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<TaskListInstanceDTO>> GetAllTaskListInstancesAsync()
        {
            var taskListInstances = await _dbContext.TaskListInstance
                .Include(tli => tli.TaskListTemplate)
                .Include(tli => tli.AssignedPerson)
                .ToListAsync();

            var taskListInstanceDTOs = taskListInstances.Select(tli => new TaskListInstanceDTO
            {
                ID = tli.ID,
                TaskListTemplateID = tli.TaskListTemplateID,
                StartDate = tli.StartDate,
                AssignedTo = tli.AssignedTo,
                Status = tli.Status,
                DueDate = tli.DueDate,

                TaskListTemplate = tli.TaskListTemplate != null ? new TaskListTemplateDTO
                {
                    ID = tli.TaskListTemplate.ID,
                    TempName = tli.TaskListTemplate.TempName,
                    CreatedDate = tli.TaskListTemplate.CreatedDate,
                    CreatedBy = tli.TaskListTemplate.CreatedBy,

                    CreatedByPerson = tli.TaskListTemplate.CreatedByPerson != null ? new PersonDTO
                    {
                        FName = tli.TaskListTemplate.CreatedByPerson.FName,
                        LName = tli.TaskListTemplate.CreatedByPerson.LName,
                        Gender = tli.TaskListTemplate.CreatedByPerson.Gender,
                        BDate = tli.TaskListTemplate.CreatedByPerson.BDate,
                        Username = tli.TaskListTemplate.CreatedByPerson.Username,
                        Password = tli.TaskListTemplate.CreatedByPerson.Password
                    } : null
                }:null,
                AssignedPerson = tli.AssignedPerson != null ? new PersonDTO
                {
                    FName = tli.AssignedPerson.FName,
                    LName = tli.AssignedPerson.LName,
                    Gender = tli.AssignedPerson.Gender,
                    BDate = tli.AssignedPerson.BDate,
                    Username = tli.AssignedPerson.Username,
                    Password = tli.AssignedPerson.Password,
                }:null
            }).ToList();

            return taskListInstanceDTOs;
        }

        public async Task<TaskListInstanceDTO> AddTaskListInstanceAsync(AddTaskListInstanceDTO addTaskListInstanceDTO)
        {
            var template = new TaskListInstance
            {
                TaskListTemplateID = addTaskListInstanceDTO.TaskListTemplateID,
                StartDate = addTaskListInstanceDTO.StartDate,
                AssignedTo = addTaskListInstanceDTO.AssignedTo,
                DueDate = addTaskListInstanceDTO.DueDate,
                Status = addTaskListInstanceDTO.Status,
            };

            _dbContext.TaskListInstance.Add(template);
            await _dbContext.SaveChangesAsync();

            return new TaskListInstanceDTO
            {
                ID = template.ID,
                TaskListTemplateID = template.TaskListTemplateID,
                StartDate = template.StartDate,
                AssignedTo = template.AssignedTo,
                DueDate = template.DueDate,
                Status = template.Status,
            };
        }

        public async Task<TaskListInstanceDTO> UpdateTaskListInstanceAsync(int ID, AddTaskListInstanceDTO addTaskListInstanceDTO)
        {
            var template = _dbContext.TaskListInstance.Find(ID);

            if (template == null)
            {
                return null;
            }

            template.TaskListTemplateID = addTaskListInstanceDTO.TaskListTemplateID;
            template.StartDate = addTaskListInstanceDTO.StartDate;
            template.AssignedTo = addTaskListInstanceDTO.AssignedTo;
            template.DueDate = addTaskListInstanceDTO.DueDate;
            template.Status = addTaskListInstanceDTO.Status;

            await _dbContext.SaveChangesAsync();

            return new TaskListInstanceDTO
            {
                ID = template.ID,
                TaskListTemplateID = template.TaskListTemplateID,
                StartDate = template.StartDate,
                AssignedTo = template.AssignedTo,
                DueDate = template.DueDate,
                Status = template.Status,
            };
        }

        public async Task<TaskListInstanceDTO> GetTaskListInstanceByIDAsync(int ID)
        {
            var taskListInstance = await _dbContext.TaskListInstance
            .Include(tli => tli.TaskListTemplate)
            .ThenInclude(tlt => tlt.CreatedByPerson)
            .Include(tli => tli.AssignedPerson)
            .FirstOrDefaultAsync(tli => tli.ID == ID);

            if (taskListInstance == null)
            {
                return null;
            }

            return new TaskListInstanceDTO
            {
                ID = taskListInstance.ID,
                TaskListTemplateID = taskListInstance.TaskListTemplateID,
                StartDate = taskListInstance.StartDate,
                AssignedTo = taskListInstance.AssignedTo,
                Status = taskListInstance.Status,
                DueDate = taskListInstance.DueDate,
                TaskListTemplate = taskListInstance.TaskListTemplate != null
                    ? new TaskListTemplateDTO
                    {
                        ID = taskListInstance.TaskListTemplate.ID,
                        TempName = taskListInstance.TaskListTemplate.TempName,
                        CreatedDate = taskListInstance.TaskListTemplate.CreatedDate,
                        CreatedBy = taskListInstance.TaskListTemplate.CreatedBy,
                        CreatedByPerson = taskListInstance.TaskListTemplate.CreatedByPerson != null
                            ? new PersonDTO
                            {
                                FName = taskListInstance.TaskListTemplate.CreatedByPerson.FName,
                                LName = taskListInstance.TaskListTemplate.CreatedByPerson.LName,
                                Gender = taskListInstance.TaskListTemplate.CreatedByPerson.Gender,
                                BDate = taskListInstance.TaskListTemplate.CreatedByPerson.BDate,
                                Username = taskListInstance.TaskListTemplate.CreatedByPerson.Username,
                                Password = taskListInstance.TaskListTemplate.CreatedByPerson.Password
                            }
                            : null
                    } : null,

                AssignedPerson = taskListInstance.AssignedPerson != null
                    ? new PersonDTO
                    {
                        FName = taskListInstance.AssignedPerson.FName,
                        LName = taskListInstance.AssignedPerson.LName,
                        Gender = taskListInstance.AssignedPerson.Gender,
                        BDate = taskListInstance.AssignedPerson.BDate,
                        Username = taskListInstance.AssignedPerson.Username,
                        Password = taskListInstance.AssignedPerson.Password
                    }
                    : null,
            };
        }

        public async Task<bool> DeleteTaskListInstanceByIDAsync(int ID)
        {
            var template = await _dbContext.TaskListInstance.FindAsync(ID);
            if (template == null)
            {
                return false;
            }

            _dbContext.TaskListInstance.Remove(template);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
