using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class AddTaskDTO
    {
        public required string TaskName { get; set; }
        public required string Description { get; set; }
        public required int TaskGroupID { get; set; }
        public int? DependancyTaskID { get; set; }
    }
}
