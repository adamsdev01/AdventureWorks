using AdventureWorksApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace AdventureWorksApp.Data.Services.AdventureWorksServices
{
    public class PersonsService
    {
        private readonly AdventureWorksContext _context;

        public PersonsService(AdventureWorksContext context)
        {
            _context = context;
        }

        #region Person Data
        /// <summary>
        /// Read lots of data in the grid
        /// </summary>
        /// <param name="queryAttrib"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> ReadPersonsByQuerying(DataSourceRequest queryAttrib)
        {
            queryAttrib.Filters.Add(new FilterDescriptor
            {
                Member = "ActiveFlag",
                Value = "Y"
            });

            var personsList = await _context.Person
                .Include(p => p.BusinessEntityContact)
                .Include(p => p.Customer)
                .Include(p => p.EmailAddress)
                .Include(p => p.PersonCreditCard)
                .Include(p => p.PersonPhone)
                .OrderByDescending(p => p.ModifiedDate)
                .ToDataSourceResultAsync(queryAttrib);

            return personsList;
        }

        /// <summary>
        /// Get Person by Id
        /// </summary>
        /// <param name="businessEntityId"></param>
        /// <returns></returns>
        public Person GetPersonById(int businessEntityId)
        {
            var person = _context.Person
                .Include(p => p.BusinessEntityContact)
                .Include(p => p.Customer)
                .Include(p => p.EmailAddress)
                .Include(p => p.PersonCreditCard)
                .Include(p => p.PersonPhone)
                .Where(e => e.BusinessEntityId == businessEntityId && e.ActiveFlag == "Y")
                .FirstOrDefault();

            return person;
        }

        /// <summary>
        /// Create a new Person
        /// </summary>
        /// <param name="Person"></param>
        /// <returns></returns>
        public async Task<Person> CreateEmployee(Person person)
        {
            person.ActiveFlag = "Y";
            person.ModifiedDate = DateTime.Now;

            await _context.AddAsync(person);
            await _context.SaveChangesAsync();

            return person;
        }

        /// <summary>
        /// Update an Person obj
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public async Task<Person> UpdateEmployee(Person person)
        {
            person.ActiveFlag = "N";
            person.ModifiedDate = DateTime.Now;

            await _context.AddAsync(person);
            await _context.SaveChangesAsync();

            return person;
        }

        /// <summary>
        /// Set Active Flag to N and remove from Grid
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public Person DeleteEmployee(Person person)
        {
            person.ModifiedDate = DateTime.Now;
            person.ActiveFlag = "N";

            _context.SaveChanges();

            return person;
        }

        /// <summary>
        /// Get List of People
        /// </summary>
        /// <returns></returns>
        public List<Person> GetPeople()
        {
            var people = _context.Person
                .ToList();

            return people;
        }
    }
    #endregion
}
