using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = "";
        public int TaskListTemplateID { get; set; }
        public TaskListTemplate TaskListTemplate { get; set; }
        public int GroupOrder { get; set; }
    }
}
