using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Filters.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Abstracts
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();
        Task<Page<Employee>> GetAllSearchedPaginatedSortedAsync(string query, string sort, int? page, int? pageSize);
        Task<List<Employee>> GetAllSearchedAsync(string query);
        Task<Page<Employee>> GetAllPaginatedAsync(int page, int pageSize);
        Task<List<Employee>> GetAllSortedAsync(string query);
        Task<Employee> GetAsync(int id);
        Task CreateAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);
    }
}
