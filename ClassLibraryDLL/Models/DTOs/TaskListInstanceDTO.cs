using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class TaskListInstanceDTO
    {
        public int ID { get; set; }
        public int TaskListTemplateID { get; set; }
        public DateTime StartDate { get; set; }
        public int AssignedTo { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public TaskListTemplateDTO? TaskListTemplate { get; set; }
        public PersonDTO? AssignedPerson { get; set; }
    }
}
