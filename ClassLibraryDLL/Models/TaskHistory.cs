using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskHistory
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public int TaskGroupID { get; set; }
        public int TaskOrder {  get; set; }
        public int? DependancyTaskID { get; set; }
        public int ChangedBy {  get; set; }
        public DateTime ChangedDate { get; set; }
        public string? ChangedType { get; set; }
        public TaskGroup? TaskGroup { get; set; }
        public Task? DependancyTask { get; set; }
        public Person? ChangedByPerson { get; set; }
    }
}
