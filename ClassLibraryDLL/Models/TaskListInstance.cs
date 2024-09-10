using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskListInstance
    {
        public int ID { get; set; }
        public int TaskListTemplateID { get; set; }
        public TaskListTemplate TaskListTemplate { get; set; }
        public DateTime StartDate { get; set; }
        public int AssignedTo { get; set; }
        public Person AssignedPerson { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}
