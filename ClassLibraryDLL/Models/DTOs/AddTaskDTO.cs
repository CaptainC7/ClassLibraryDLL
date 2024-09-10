using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class AddTaskDTO
    {
        public string TaskName { get; set; } = "";
        public string Description { get; set; } = "";
        public int TaskGroupID { get; set; }
        public int? DependancyTaskID { get; set; }
    }
}
