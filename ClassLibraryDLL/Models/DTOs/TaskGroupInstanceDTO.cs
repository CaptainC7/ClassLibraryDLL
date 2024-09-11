using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class TaskGroupInstanceDTO
    {
        public int ID { get; set; }
        public int TaskGroupID { get; set; }
        public TaskGroupDTO? TaskGroup { get; set; }
        public int TaskListInstanceID { get; set; }
        public TaskListInstanceDTO? TaskListInstance { get; set; }
        public int AssignedTo { get; set; }
        public PersonDTO? AssignedPerson { get; set; }
        public String? Status { get; set; }
    }
}
