using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class AddTaskListTemplateDTO
    {
        public required string TempName { get; set; }
        public required DateOnly CreatedDate { get; set; }
        public required int CreatedBy { get; set; }
    }
}
