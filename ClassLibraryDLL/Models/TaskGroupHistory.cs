using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models
{
    public class TaskGroupHistory
    {
        public int ID { get; set; }
        public int TaskGroupID { get; set; }
        public string? GroupName { get; set; }
        public int TaskListTemplateID { get; set; }
        public int GroupOrder {  get; set; }
        public int ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string? ChangedType { get; set; }
        public Person? ChangedByPerson { get; set; }
    }
}
