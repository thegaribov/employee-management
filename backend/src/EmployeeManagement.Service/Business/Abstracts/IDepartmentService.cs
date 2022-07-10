using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Filters;
using EmployeeManagement.Core.Filters.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Abstracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentForCollectionDTO>> GetAllAsync(DepartmentsQueryParams queryParams);
        Task<DepartmentDetailsDTO> GetDetailsAsync(int id);
        Task<CreateDepartmentResponseDTO> CreateAsync(CreateDepartmentRequestDTO departmentDTO);
        Task UpdateAsync(int id, UpdateDepartmentDTO departmentDTO);
        Task DeleteAsync(int id);

        Task<IEnumerable<EmployeeForCollectionDTO>> GetEmployeesAsync(int departmentId, QueryParams queryParams);
        Task<EmployeeDetailsResponseDTO> GetEmployeeDetailsAsync(int departmentId, int employeeId);
    }
}
