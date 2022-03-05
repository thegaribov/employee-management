using EmployeeManagement.Core.Pagination.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Abstracts
{
    public interface IDepartmentService
    {
        Task<List<Core.Entities.Department>> GetAllAsync();
        Task<Paginator<Core.Entities.Department>> GetAllSearchedPaginatedSortedAsync(string query, string sort, int? page, int? pageSize);
        Task<Core.Entities.Department> GetAsync(int id);
        Task CreateAsync(Core.Entities.Department department);
        Task UpdateAsync(Core.Entities.Department department);
        Task DeleteAsync(Core.Entities.Department department);
    }
}
