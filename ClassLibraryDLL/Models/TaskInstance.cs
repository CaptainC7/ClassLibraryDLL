using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskInstance
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public int TaskGroupInstanceID { get; set; }
        public string? Status { get; set; }
        public DateTime? TaskCompletionDate { get; set; }
        public Task? Task { get; set; }
        public TaskGroupInstance? TaskGroupInstance { get; set; }
        public ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();
    }
}
