using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskListTemplateHistory
    {
        public int ID { get; set; }
        public int TaskListTemplateID { get; set; }
        public string? TempName { get; set; }
        public int CreatedBy { get; set; }
        public Person? CreatedByPerson { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ChangedBy { get; set; }
        public Person? ChangedByPerson { get; set; }
        public DateTime ChangedDate { get; set; }
        public string? ChangedType { get; set; }
    }
}
