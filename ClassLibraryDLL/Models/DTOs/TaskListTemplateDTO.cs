using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class TaskListTemplateDTO
    {
        public required string  TempName { get; set; }
        public required DateOnly CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public PersonDTO? CreatedByPerson { get; set; }
    }
}
