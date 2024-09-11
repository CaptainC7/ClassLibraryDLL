using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class PersonDTO
    {
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Gender { get; set; }
        public DateOnly BDate { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
