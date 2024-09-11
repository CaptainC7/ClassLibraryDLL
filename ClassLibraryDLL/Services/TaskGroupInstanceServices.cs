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
    public class TaskGroupInstanceServices : ITaskGroupInstanceServices
    {
        private readonly ApplicationDBContext _dbContext;
        public TaskGroupInstanceServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<TaskGroupInstanceDTO>> GetTaskGroupInstancesByTaskListInstanceID(int taskListInstanceId)
        {
            return await _dbContext.TaskGroupInstance
                .Where(tgi => tgi.TaskListInstanceID == taskListInstanceId)
                .Include(tgi => tgi.TaskGroup)
                .Include(tgi => tgi.TaskListInstance)
                .Include(tgi => tgi.AssignedPerson)
                .Select(tgi => new TaskGroupInstanceDTO
                {
                    ID = tgi.ID,
                    TaskGroupID = tgi.TaskGroupID,
                    TaskGroup = new TaskGroupDTO
                    {
                        Id = tgi.TaskGroup.Id,
                        GroupName = tgi.TaskGroup.GroupName,
                        TaskListTemplateID = tgi.TaskGroup.TaskListTemplateID,
                        GroupOrder = tgi.TaskGroup.GroupOrder,
                    },
                    TaskListInstanceID = tgi.TaskListInstanceID,
                    TaskListInstance = new TaskListInstanceDTO
                    {
                        ID = tgi.TaskListInstance.ID,
                        TaskListTemplateID = tgi.TaskListInstance.TaskListTemplateID,
                        StartDate = tgi.TaskListInstance.StartDate,
                        AssignedTo = tgi.TaskListInstance.AssignedTo,
                        Status = tgi.TaskListInstance.Status,
                        DueDate = tgi.TaskListInstance.DueDate,
                        TaskListTemplate = new TaskListTemplateDTO
                        {
                            ID = tgi.TaskListInstance.TaskListTemplate.ID,
                            TempName = tgi.TaskListInstance.TaskListTemplate.TempName,
                            CreatedDate = tgi.TaskListInstance.TaskListTemplate.CreatedDate,
                            CreatedBy = tgi.TaskListInstance.TaskListTemplate.CreatedBy
                        },
                        AssignedPerson = new PersonDTO
                        {
                            FName = tgi.TaskListInstance.AssignedPerson.FName,
                            LName = tgi.TaskListInstance.AssignedPerson.LName,
                            Gender = tgi.TaskListInstance.AssignedPerson.Gender,
                            BDate = tgi.TaskListInstance.AssignedPerson.BDate,
                            Username = tgi.TaskListInstance.AssignedPerson.Username,
                            Password = tgi.TaskListInstance.AssignedPerson.Password
                        }
                    },
                    AssignedTo = tgi.AssignedTo,
                    AssignedPerson = new PersonDTO
                    {
                        FName = tgi.AssignedPerson.FName,
                        LName = tgi.AssignedPerson.LName,
                        Gender = tgi.AssignedPerson.Gender,
                        BDate = tgi.AssignedPerson.BDate,
                        Username = tgi.AssignedPerson.Username,
                        Password = tgi.AssignedPerson.Password
                    },
                    Status = tgi.Status
                })
                .ToListAsync();
        }

        public async Task<TaskGroupInstanceDTO> AddTaskGroupInstance(AddTaskGroupInstanceDTO addTaskGroupInstanceDTO)
        {
            var newTaskGroupInstance = new TaskGroupInstance
            {
                TaskGroupID = addTaskGroupInstanceDTO.TaskGroupID,
                TaskListInstanceID = addTaskGroupInstanceDTO.TaskListInstanceID,
                AssignedTo = addTaskGroupInstanceDTO.AssignedTo,
                Status = addTaskGroupInstanceDTO.Status
            };

            _dbContext.TaskGroupInstance.Add(newTaskGroupInstance);
            await _dbContext.SaveChangesAsync();

            var insertedTaskGroupInstance = await _dbContext.TaskGroupInstance
                .Include(tgi => tgi.TaskGroup)
                .Include(tgi => tgi.TaskListInstance)
                .ThenInclude(tli => tli.TaskListTemplate)
                .Include(tgi => tgi.AssignedPerson)
                .FirstOrDefaultAsync(tgi => tgi.ID == newTaskGroupInstance.ID);

            if (insertedTaskGroupInstance == null)
            {
                throw new Exception("TaskGroupInstance insertion failed or related data could not be fetched.");
            }

            return new TaskGroupInstanceDTO
            {
                ID = insertedTaskGroupInstance.ID,
                TaskGroupID = insertedTaskGroupInstance.TaskGroupID,
                TaskGroup = insertedTaskGroupInstance.TaskGroup != null ? new TaskGroupDTO
                {
                    Id = insertedTaskGroupInstance.TaskGroup.Id,
                    GroupName = insertedTaskGroupInstance.TaskGroup.GroupName,
                    TaskListTemplateID = insertedTaskGroupInstance.TaskGroup.TaskListTemplateID,
                    GroupOrder = insertedTaskGroupInstance.TaskGroup.GroupOrder
                } : null,
                TaskListInstanceID = insertedTaskGroupInstance.TaskListInstanceID,
                TaskListInstance = insertedTaskGroupInstance.TaskListInstance != null ? new TaskListInstanceDTO
                {
                    ID = insertedTaskGroupInstance.TaskListInstance.ID,
                    TaskListTemplateID = insertedTaskGroupInstance.TaskListInstance.TaskListTemplateID,
                    StartDate = insertedTaskGroupInstance.TaskListInstance.StartDate,
                    AssignedTo = insertedTaskGroupInstance.TaskListInstance.AssignedTo,
                    Status = insertedTaskGroupInstance.TaskListInstance.Status,
                    DueDate = insertedTaskGroupInstance.TaskListInstance.DueDate,
                    TaskListTemplate = insertedTaskGroupInstance.TaskListInstance.TaskListTemplate != null ? new TaskListTemplateDTO
                    {
                        ID = insertedTaskGroupInstance.TaskListInstance.TaskListTemplate.ID,
                        TempName = insertedTaskGroupInstance.TaskListInstance.TaskListTemplate.TempName,
                        CreatedDate = insertedTaskGroupInstance.TaskListInstance.TaskListTemplate.CreatedDate,
                        CreatedBy = insertedTaskGroupInstance.TaskListInstance.TaskListTemplate.CreatedBy
                    } : null
                } : null,
                AssignedTo = insertedTaskGroupInstance.AssignedTo,
                AssignedPerson = insertedTaskGroupInstance.AssignedPerson != null ? new PersonDTO
                {
                    FName = insertedTaskGroupInstance.AssignedPerson.FName,
                    LName = insertedTaskGroupInstance.AssignedPerson.LName,
                    Gender = insertedTaskGroupInstance.AssignedPerson.Gender,
                    BDate = insertedTaskGroupInstance.AssignedPerson.BDate,
                    Username = insertedTaskGroupInstance.AssignedPerson.Username,
                    Password = insertedTaskGroupInstance.AssignedPerson.Password
                } : null,
                Status = insertedTaskGroupInstance.Status
            };
        }

        public async Task<TaskGroupInstanceDTO> UpdateTaskGroupInstance(int ID, AddTaskGroupInstanceDTO addTaskGroupInstanceDTO)
        {
            var existingTaskGroupInstance = await _dbContext.TaskGroupInstance
            .Include(tgi => tgi.TaskGroup)
            .Include(tgi => tgi.TaskListInstance)
            .Include(tgi => tgi.AssignedPerson)
            .FirstOrDefaultAsync(tgi => tgi.ID == ID);

            if (existingTaskGroupInstance == null)
            {
                return null; // TaskGroupInstance not found
            }

            // Update properties
            existingTaskGroupInstance.TaskGroupID = addTaskGroupInstanceDTO.TaskGroupID;
            existingTaskGroupInstance.TaskListInstanceID = addTaskGroupInstanceDTO.TaskListInstanceID;
            existingTaskGroupInstance.AssignedTo = addTaskGroupInstanceDTO.AssignedTo;
            existingTaskGroupInstance.Status = addTaskGroupInstanceDTO.Status;

            // Save changes
            await _dbContext.SaveChangesAsync();

            return new TaskGroupInstanceDTO
            {
                ID = existingTaskGroupInstance.ID,
                TaskGroupID = existingTaskGroupInstance.TaskGroupID,
                TaskGroup = existingTaskGroupInstance.TaskGroup != null ? new TaskGroupDTO
                {
                    Id = existingTaskGroupInstance.TaskGroup.Id,
                    GroupName = existingTaskGroupInstance.TaskGroup.GroupName,
                    TaskListTemplateID = existingTaskGroupInstance.TaskGroup.TaskListTemplateID,
                    GroupOrder = existingTaskGroupInstance.TaskGroup.GroupOrder
                } : null,
                TaskListInstanceID = existingTaskGroupInstance.TaskListInstanceID,
                TaskListInstance = existingTaskGroupInstance.TaskListInstance != null ? new TaskListInstanceDTO
                {
                    ID = existingTaskGroupInstance.TaskListInstance.ID,
                    TaskListTemplateID = existingTaskGroupInstance.TaskListInstance.TaskListTemplateID,
                    StartDate = existingTaskGroupInstance.TaskListInstance.StartDate,
                    AssignedTo = existingTaskGroupInstance.TaskListInstance.AssignedTo,
                    Status = existingTaskGroupInstance.TaskListInstance.Status,
                    DueDate = existingTaskGroupInstance.TaskListInstance.DueDate,
                    TaskListTemplate = existingTaskGroupInstance.TaskListInstance.TaskListTemplate != null ? new TaskListTemplateDTO
                    {
                        ID = existingTaskGroupInstance.TaskListInstance.TaskListTemplate.ID,
                        TempName = existingTaskGroupInstance.TaskListInstance.TaskListTemplate.TempName,
                        CreatedDate = existingTaskGroupInstance.TaskListInstance.TaskListTemplate.CreatedDate,
                        CreatedBy = existingTaskGroupInstance.TaskListInstance.TaskListTemplate.CreatedBy
                    } : null
                } : null,
                AssignedTo = existingTaskGroupInstance.AssignedTo,
                AssignedPerson = existingTaskGroupInstance.AssignedPerson != null ? new PersonDTO
                {
                    FName = existingTaskGroupInstance.AssignedPerson.FName,
                    LName = existingTaskGroupInstance.AssignedPerson.LName,
                    Gender = existingTaskGroupInstance.AssignedPerson.Gender,
                    BDate = existingTaskGroupInstance.AssignedPerson.BDate,
                    Username = existingTaskGroupInstance.AssignedPerson.Username,
                    Password = existingTaskGroupInstance.AssignedPerson.Password
                } : null,
                Status = existingTaskGroupInstance.Status
            };
        }

        public async Task<bool> DeleteTaskGroupInstance(int ID)
        {
            var template = await _dbContext.TaskGroupInstance.FindAsync(ID);
            if (template == null)
            {
                return false;
            }

            _dbContext.TaskGroupInstance.Remove(template);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
