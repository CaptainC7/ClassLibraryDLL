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
        private readonly IPersonHistoryServices _personHistoryService;

        public PersonServices(ApplicationDBContext dBContext, IPersonHistoryServices personHistoryService) {
            _dbContext = dBContext;
            _personHistoryService = personHistoryService;
        }

        public async Task<IEnumerable<Person>> GetUsers()
        {
            var users = await _dbContext.Person.ToListAsync();
            return users;
        }

        public async Task<PersonDTO> AddPerson(PersonDTO personDTO, int userID)
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

            var personHistoryDTO = new PersonHistoryDTO
            {
                PersonID = person.ID,
                FName = person.FName,
                LName = person.LName,
                Gender = person.Gender,
                BDate = person.BDate,
                Username = person.Username,
                Password = person.Password,
                ChangedBy = userID,
                ChangeDate = DateTime.Now,
                ChangeType = "Create"
            };

            await _personHistoryService.AddPersonHistoryAsync(personHistoryDTO);

            return personDTO;
        }

        public async Task<PersonDTO> UpdatePerson(int id, PersonDTO personDTO, int userID)
        {
            var user = await _dbContext.Person.FindAsync(id);

            if (user == null) {
                return null;
            }

            var personHistoryDTO = new PersonHistoryDTO
            {
                PersonID = user.ID,
                FName = user.FName,
                LName = user.LName,
                Gender = user.Gender,
                BDate = user.BDate,
                Username = user.Username,
                Password = user.Password,
                ChangedBy = userID,
                ChangeDate = DateTime.Now,
                ChangeType = "Update"
            };

            await _personHistoryService.AddPersonHistoryAsync(personHistoryDTO);

            user.FName = personDTO.FName;
            user.LName = personDTO.LName;
            user.Gender = personDTO.Gender;
            user.BDate = personDTO.BDate;
            user.Username = personDTO.Username;
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(personDTO.Password, 13);

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

        public async Task<bool> DeletePersonByID(int id, int userID)
        {
            var person = await _dbContext.Person.FindAsync(id);
            if(person == null)
            {
                return false;
            }

            var personHistoryDTO = new PersonHistoryDTO
            {
                PersonID = person.ID,
                FName = person.FName,
                LName = person.LName,
                Gender = person.Gender,
                BDate = person.BDate,
                Username = person.Username,
                Password = person.Password,
                ChangedBy = userID,
                ChangeDate = DateTime.Now,
                ChangeType = "Delete"
            };

            await _personHistoryService.AddPersonHistoryAsync(personHistoryDTO);

            _dbContext.Person.Remove(person);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
