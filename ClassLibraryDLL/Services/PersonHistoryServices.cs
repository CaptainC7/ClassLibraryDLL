using ClassLibraryDLL.Models;
using ClassLibraryDLL.Models.ApplicationDBContext;
using ClassLibraryDLL.Models.DTOs;
using ClassLibraryDLL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ClassLibraryDLL.Services
{
    public class PersonHistoryServices : IPersonHistoryServices
    {
        private readonly ApplicationDBContext _dbContext;

        public PersonHistoryServices(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPersonHistoryAsync(PersonHistoryDTO personHistoryDTO)
        {
            var personHistory = new PersonHistory
            {
                PersonID = personHistoryDTO.PersonID,
                FName = personHistoryDTO.FName,
                LName = personHistoryDTO.LName,
                Gender = personHistoryDTO.Gender,
                BDate = personHistoryDTO.BDate,
                Username = personHistoryDTO.Username,
                Password = personHistoryDTO.Password,
                ChangeDate = personHistoryDTO.ChangeDate,
                ChangeType = personHistoryDTO.ChangeType
            };

            _dbContext.PersonHistory.Add(personHistory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonHistoryDTO>> GetAllPersonHistoriesAsync()
        {
            var histories = await _dbContext.PersonHistory
            .Include(h => h.ChangedByPerson)
            .Select(h => new PersonHistoryDTO
            {
                ID = h.ID,
                PersonID = h.PersonID,
                FName = h.FName,
                LName = h.LName,
                Gender = h.Gender,
                BDate = h.BDate,
                Username = h.Username,
                Password = h.Password,
                ChangeDate = h.ChangeDate,
                ChangeType = h.ChangeType,
                ChangedBy = h.ChangedBy,
                ChangedByPerson = new PersonDTO
                {
                    FName = h.ChangedByPerson.FName,
                    LName = h.ChangedByPerson.LName,
                    Gender = h.ChangedByPerson.Gender,
                    BDate = h.ChangedByPerson.BDate,
                    Username = h.ChangedByPerson.Username
                }
            })
            .ToListAsync();

            return histories;
        }
    }
}
