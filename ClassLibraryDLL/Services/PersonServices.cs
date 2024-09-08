using ClassLibraryDLL.Models;
using ClassLibraryDLL.Models.ApplicationDBContext;
using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Services
{
    public class PersonServices : IPersonServices
    {
        private readonly ApplicationDBContext _dbContext;

        public PersonServices(ApplicationDBContext dBContext) {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<Person>> GetUsers()
        {
            var users = await _dbContext.Person.ToListAsync();
            return users;
        }

        public async Task<PersonDTO> AddPerson(PersonDTO personDTO)
        {
            var person = new Person
            {
                FName = personDTO.FName,
                LName = personDTO.LName,
                Gender = personDTO.Gender,
                BDate = personDTO.BDate,
                Username = personDTO.Username,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(personDTO.Password, 13),
            };

            _dbContext.Person.Add(person);
            await _dbContext.SaveChangesAsync();
            return personDTO;
        }

        public async Task<PersonDTO> UpdatePerson(int id, PersonDTO personDTO)
        {
            var user = _dbContext.Person.Find(id);

            if (user == null) {
                return null;
            }

            user.FName = personDTO.FName;
            user.LName = personDTO.LName;
            user.Gender = personDTO.Gender;
            user.BDate = personDTO.BDate;
            user.Username = personDTO.Username;
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(personDTO.Password, 13);
            Console.WriteLine(BCrypt.Net.BCrypt.EnhancedVerify(personDTO.Password, user.Password));
            await _dbContext.SaveChangesAsync();


            return personDTO;
        }

        public bool CheckLogin(string username, string password)
        {
            var user = _dbContext.Person.Single(s => s.Username == username);
            if(BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
            {
                return true;
            } else { return false; }
        }

        public async Task<Person> GetPersonByID(int ID)
        {
            var person = _dbContext.Person.Find(ID);
            if(person == null)
            {
                return null;
            }
            return person;
        }

        public async Task<bool> DeletePersonByID(int id)
        {
            var person = await _dbContext.Person.FindAsync(id);
            if(person == null)
            {
                return false;
            }

            _dbContext.Person.Remove(person);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
