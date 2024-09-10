using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class AddTaskListInstanceDTO
    {
        public int TaskListTemplateID { get; set; }
        public DateTime StartDate { get; set; }
        public int AssignedTo { get; set; }
        public DateTime DueDate { get; set; }
    }
}
