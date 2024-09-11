using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class AddTaskListInstanceDTO
    {
        public required int TaskListTemplateID { get; set; }
        public required DateTime StartDate { get; set; }
        public required int AssignedTo { get; set; }
        public required DateTime DueDate { get; set; }
        public required string Status { get; set; }
    }
}
