using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskGroupInstance
    {
        public int ID { get; set; }
        public int TaskGroupID { get; set; }
        public TaskGroup? TaskGroup { get; set; }
        public int TaskListInstanceID { get; set; }
        public TaskListInstance? TaskListInstance { get; set; }
        public int AssignedTo { get; set; }
        public Person? AssignedPerson { get; set; }
        public String? Status { get; set; }
        public ICollection<TaskInstance> TaskInstances { get; set; } = new List<TaskInstance>();
    }
}
