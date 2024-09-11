using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class AddTaskInstanceDTO
    {
        public required int TaskID { get; set; }
        public required int TaskGroupInstanceID { get; set; }
        public required string? Status { get; set; }
    }
}
