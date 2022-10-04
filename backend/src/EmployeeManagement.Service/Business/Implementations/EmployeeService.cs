using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Filters.Pagination;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.Service.Business.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeManagementContext _dbContext;

        public EmployeeService(EmployeeManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbContext.Employees.ToListAsync();
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
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task CreateAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
