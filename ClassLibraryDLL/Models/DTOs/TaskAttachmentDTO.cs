using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class TaskAttachmentDTO
    {
        public int ID { get; set; }
        public int TaskInstanceID { get; set; }
        public string? FilePath { get; set; }
        public int UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public PersonDTO? UploadedByPerson { get; set; }
    }
}
