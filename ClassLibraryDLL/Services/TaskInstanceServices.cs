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
    public class TaskInstanceServices : ITaskInstanceServices
    {
        private readonly ApplicationDBContext _dbContext;
        public TaskInstanceServices(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<TaskInstanceDTO>> GetTaskInstancebyTaskGroupInstanceID(int taskGroupInstanceID)
        {
            var taskInstances = await _dbContext.TaskInstance
                .Include(ti => ti.Task)
                .Where(ti => ti.TaskGroupInstanceID == taskGroupInstanceID)
                .ToListAsync();

            return taskInstances.Select(ti => new TaskInstanceDTO
            {
                ID = ti.ID,
                TaskID = ti.TaskID,
                TaskGroupInstanceID = ti.TaskGroupInstanceID,
                Status = ti.Status,
                //TaskCompletionDate = ti.TaskCompletionDate,
                Task = new TaskDTO
                {
                    ID = ti.Task.ID,
                    TaskName = ti.Task.TaskName,
                    Description = ti.Task.Description,
                    TaskGroupID = ti.Task.TaskGroupID,
                    TaskOrder = ti.Task.TaskOrder,
                    DependancyTaskID = ti.Task.DependancyTaskID,
                }
            });
        }

        public async Task<TaskInstanceDTO> AddTaskInstance(AddTaskInstanceDTO addTaskInstanceDTO)
        {
            var newTaskInstance = new TaskInstance
            {
                TaskID = addTaskInstanceDTO.TaskID,
                TaskGroupInstanceID = addTaskInstanceDTO.TaskGroupInstanceID,
                Status = addTaskInstanceDTO.Status,
            };

            _dbContext.TaskInstance.Add(newTaskInstance);
            await _dbContext.SaveChangesAsync();

            var addedTaskInstance = await _dbContext.TaskInstance
                .Include(ti => ti.Task)
                .FirstOrDefaultAsync(ti => ti.ID == newTaskInstance.ID);

            if (addedTaskInstance == null)
            {
                return null;
            }

            return new TaskInstanceDTO
            {
                ID = addedTaskInstance.ID,
                TaskID = addedTaskInstance.TaskID,
                TaskGroupInstanceID = addedTaskInstance.TaskGroupInstanceID,
                Status = addedTaskInstance.Status,
                TaskCompletionDate = addedTaskInstance.TaskCompletionDate,
                Task = new TaskDTO
                {
                    ID = addedTaskInstance.Task.ID,
                    TaskName = addedTaskInstance.Task.TaskName,
                    Description = addedTaskInstance.Task.Description,
                    TaskGroupID = addedTaskInstance.Task.TaskGroupID,
                    TaskOrder = addedTaskInstance.Task.TaskOrder,
                    DependancyTaskID = addedTaskInstance.Task.DependancyTaskID,
                }
            };
        }

        public async Task<TaskInstanceDTO> UpdateTaskInstance(int id, UpdateTaskInstanceDTO updateTaskInstanceDTO)
        {
            var taskInstance = await _dbContext.TaskInstance
            .FirstOrDefaultAsync(ti => ti.ID == id);

            if (taskInstance == null)
            {
                throw new Exception("TaskInstance not found.");
            }

            taskInstance.TaskID = updateTaskInstanceDTO.TaskID;
            taskInstance.TaskGroupInstanceID = updateTaskInstanceDTO.TaskGroupInstanceID;
            taskInstance.Status = updateTaskInstanceDTO.Status;

            if (string.IsNullOrEmpty(updateTaskInstanceDTO.TaskCompletionDate?.ToString()))
            {
                taskInstance.TaskCompletionDate = null;
            }
            else
            {
                taskInstance.TaskCompletionDate = updateTaskInstanceDTO.TaskCompletionDate;
            }

            await _dbContext.SaveChangesAsync();

            return new TaskInstanceDTO
            {
                ID = taskInstance.ID,
                TaskID = taskInstance.TaskID,
                TaskGroupInstanceID = taskInstance.TaskGroupInstanceID,
                Status = taskInstance.Status,
                TaskCompletionDate = taskInstance.TaskCompletionDate
            };
        }

        public async Task<bool> DeleteTaskInstance(int ID)
        {
            var template = await _dbContext.TaskInstance.FindAsync(ID);
            if (template == null)
            {
                return false;
            }

            _dbContext.TaskInstance.Remove(template);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
