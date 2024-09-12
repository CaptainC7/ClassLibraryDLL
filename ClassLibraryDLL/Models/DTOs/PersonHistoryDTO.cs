using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class PersonHistoryDTO
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Gender { get; set; }
        public DateOnly BDate { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int ChangedBy { get; set; }
        public PersonDTO? ChangedByPerson { get; set; }
        public DateTime ChangeDate { get; set; }
        public string? ChangeType { get; set; }
    }
}
