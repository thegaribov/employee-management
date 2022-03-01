using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Abstracts
{
    public interface IEmployeeService
    {
        Task<List<Core.Entities.Employee>> GetAllAsync();
        Task<Core.Entities.Employee> GetAsync(int id);
        Task CreateAsync(Core.Entities.Employee employee);
        Task UpdateAsync(Core.Entities.Employee employee);
        Task DeleteAsync(Core.Entities.Employee employee);
    }
}
