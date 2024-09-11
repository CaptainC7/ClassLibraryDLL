using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class TaskInstanceDTO
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public int TaskGroupInstanceID { get; set; }
        public string? Status { get; set; }
        public DateTime TaskCompletionDate { get; set; }
        public TaskDTO? Task { get; set; }
    }
}
