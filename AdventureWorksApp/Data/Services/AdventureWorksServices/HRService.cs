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
    public class HRService
    {
        private readonly AdventureWorksContext _context;

        public HRService(AdventureWorksContext context)
        {
            _context = context;
        }

        #region Employee Data
        /// <summary>
        /// Read lots of data in the grid
        /// </summary>
        /// <param name="queryAttrib"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> ReadEmployeeByQuerying(DataSourceRequest queryAttrib)
        {
            queryAttrib.Filters.Add(new FilterDescriptor
            {
                Member = "CurrentFlag",
                Value = true
            });

            var employeesList = await _context.Employee
                .Include(e => e.BusinessEntity)
                .Include(e => e.SalesPerson)
                .OrderByDescending(e => e.HireDate)
                .ToDataSourceResultAsync(queryAttrib);

            return employeesList;
        }

        /// <summary>
        /// Get Employee by Id
        /// </summary>
        /// <param name="businessEntityId"></param>
        /// <returns></returns>
        public Employee GetEmployeeById(int businessEntityId)
        {
            var employee = _context.Employee
                .Include(e => e.EmployeeDepartmentHistory)
                .Include(e => e.EmployeePayHistory)
                .Include(e => e.JobCandidate)
                .Include(e => e.PurchaseOrderHeader)
                .Where(e => e.BusinessEntityId == businessEntityId && e.CurrentFlag == true)
                .FirstOrDefault();

            return employee;
        }

        /// <summary>
        /// Create a new Employee
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns></returns>
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            employee.CurrentFlag = true;
            employee.ModifiedDate = DateTime.Now;

            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        /// <summary>
        /// Update an Employee obj
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            employee.CurrentFlag = true;
            employee.ModifiedDate = DateTime.Now;

            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        /// <summary>
        /// Set CurrentFlag Flag to 0 and remove from Grid
        /// </summary>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        public Employee DeleteEmployee(Employee employee)
        {
            employee.ModifiedDate = DateTime.Now;
            employee.CurrentFlag = false;

            _context.SaveChanges();

            return employee;
        }

        /// <summary>
        /// Get List of Employees
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetEmployees()
        {
            var employees = _context.Employee
                .ToList();

            return employees;
        }


        #endregion

        public Person GetPersonByEmployeeId(int businessEntityId)
        {
            var person = _context.Person
                .Include(e => e.Employee)
                .Where(e => e.BusinessEntityId == businessEntityId)
                .FirstOrDefault();

            return person;
        }
    }
}
