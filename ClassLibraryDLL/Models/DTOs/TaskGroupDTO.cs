using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class TaskGroupDTO
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }
        public int TaskListTemplateID { get; set; }
        public int GroupOrder { get; set; }
        public TaskListTemplateDTO? TaskListTemplate { get; set; }
    }
}
