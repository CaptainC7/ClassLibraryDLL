using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class AddTaskGroupInstanceDTO
    {
        public required int TaskGroupID { get; set; }
        public required int TaskListInstanceID { get; set; }
        public required int AssignedTo { get; set; }
        public required String? Status { get; set; }
    }
}
