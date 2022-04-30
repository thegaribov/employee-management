using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Filters.Base;
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
        Task<IEnumerable<DepartmentForCollectionDTO>> GetAllAsync(QueryParams queryParams);
        Task<DepartmentDetailsDTO> GetDetailsAsync(int id);
        Task<DepartmentDetailsDTO> CreateAsync(CreateDepartmentDTO departmentDTO);
        Task UpdateAsync(int id, UpdateDepartmentDTO departmentDTO);
        Task DeleteAsync(int id);

        Task<IEnumerable<EmployeeForCollectionDTO>> GetDepartmentEmployeesAsync(int departmentId, QueryParams queryParams);
        Task<DepartmentEmployeeDetailsDTO> GetDepartmentEmployeeDetailsAsync(int departmentId, int employeeId);
    }
}
