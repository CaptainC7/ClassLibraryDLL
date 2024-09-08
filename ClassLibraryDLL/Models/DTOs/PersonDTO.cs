using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.DTOs
{
    public class PersonDTO
    {
        public required string FName { get; set; }
        public required string LName { get; set; }
        public required string Gender { get; set; }
        public required DateOnly BDate { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
