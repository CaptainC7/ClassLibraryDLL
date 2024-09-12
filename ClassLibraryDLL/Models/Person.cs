using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Gender { get; set; }
        public DateOnly BDate { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public ICollection<TaskListTemplate>? CreatedTaskListTemplates { get; set; } = new List<TaskListTemplate>();
        public ICollection<TaskListInstance> AssignedTaskListInstances { get; set; } = new List<TaskListInstance>();
        public ICollection<TaskGroupInstance> TaskGroupInstances { get; set; } = new List<TaskGroupInstance>();
        public ICollection<TaskAttachment> UploadedTaskAttachments { get; set; } = new List<TaskAttachment>();
        public ICollection<TaskListTemplateHistory> CreatedTaskListTemplateHistories { get; set; } = new List<TaskListTemplateHistory>();
        public ICollection<TaskListTemplateHistory> ChangedTaskListTemplateHistories { get; set; } = new List<TaskListTemplateHistory>();
    }
}
