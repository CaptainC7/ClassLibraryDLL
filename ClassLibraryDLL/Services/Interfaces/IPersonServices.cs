using ClassLibraryDLL.Models;
using ClassLibraryDLL.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services.Interfaces
{
    public interface IPersonServices
    {
        Task<IEnumerable<Person>> GetUsers();
        Task<PersonDTO> AddPerson(PersonDTO person);
        Task<PersonDTO> UpdatePerson(int id, PersonDTO personDTO);
        Task<Person> GetPersonByID(int id);
        bool CheckLogin(string username, string password);
        Task<bool> DeletePersonByID(int id);
    }
}
