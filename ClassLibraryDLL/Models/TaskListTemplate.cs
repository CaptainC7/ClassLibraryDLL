using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskListTemplate
    {
        public int ID { get; set; }
        public string TempName { get; set; } = "";
        public int CreatedBy { get; set; }
        public DateOnly CreatedDate { get;  set; }
    }
}
