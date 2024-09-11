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
            var taskInstances = await _dbContext.taskInstance
                .Include(ti => ti.Task) // Include Task details
                .Where(ti => ti.TaskGroupInstanceID == taskGroupInstanceID)
                .ToListAsync();

            return taskInstances.Select(ti => new TaskInstanceDTO
            {
                ID = ti.ID,
                TaskID = ti.TaskID,
                TaskGroupInstanceID = ti.TaskGroupInstanceID,
                Status = ti.Status,
                TaskCompletionDate = ti.TaskCompletionDate,
                Task = new TaskDTO
                {
                    ID = ti.Task.ID,
                    TaskName = ti.Task.TaskName,
                    Description = ti.Task.Description
                }
            });
        }
    }
}
