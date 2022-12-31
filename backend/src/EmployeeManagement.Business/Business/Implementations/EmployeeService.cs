using EmployeeManagement.Business.Business.Abstracts;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Filters.Pagination;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Business.Business.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeManagementContext _dbContext;
        private readonly ICacheService _cacheService;
        private static object _lock = new object();

        public EmployeeService(
            EmployeeManagementContext dbContext,
            ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            var cachedEmployees = _cacheService.GetData<List<Employee>>("employees");
            if (cachedEmployees is not null)
            {
                return cachedEmployees;
            }

            cachedEmployees = _dbContext.Employees.ToList();
            _cacheService.SetData("employees", cachedEmployees, DateTime.Now.AddMinutes(5));


            return cachedEmployees;
        }

        public async Task<Page<Employee>> GetAllSearchedFilteredSortedPaginatedAsync(string search, string filter, string sort, int? page, int? pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Page<Employee>> GetAllPaginatedAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Employee>> GetAllSortedAsync(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> GetAsync(int id)
        {
            var cachedEmployees = _cacheService.GetData<List<Employee>>("employees");
            if (cachedEmployees is not null)
            {
                return cachedEmployees.Where(e => e.Id == id).SingleOrDefault();
            }

            return await _dbContext.Employees.Where(e => e.Id == id).SingleOrDefaultAsync();
        }

        public async Task CreateAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            _cacheService.RemoveData("employees");
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            _cacheService.RemoveData("employees");
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            _cacheService.RemoveData("employees");
            await _dbContext.SaveChangesAsync();
        }
    }
}
