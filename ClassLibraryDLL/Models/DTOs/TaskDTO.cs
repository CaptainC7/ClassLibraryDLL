using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class TaskDTO
    {
        public int ID { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public int TaskGroupID { get; set; }
        public int TaskOrder { get; set; }
        public int? DependancyTaskID { get; set; }
        public TaskGroupDTO? TaskGroup { get; set; }
    }
}
