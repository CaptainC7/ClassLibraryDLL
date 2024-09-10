using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class Task
    {
        public int ID {  get; set; }
        public string TaskName { get; set; } = "";
        public string Description { get; set; } = "";
        public int TaskGroupID { get; set; }
        public TaskGroup TaskGroup { get; set; }
        public int TaskOrder {  get; set; }
        public int? DependancyTaskID { get; set; }
    }
}
