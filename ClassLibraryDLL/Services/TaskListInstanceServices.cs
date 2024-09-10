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
            // Fetch TaskListInstance data including related TaskListTemplate and Person data
            var taskListInstances = await _dbContext.TaskListInstance
                .Include(tli => tli.TaskListTemplate)
                .Include(tli => tli.AssignedPerson)
                .ToListAsync();

            // Map to DTOs
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
    }
}
