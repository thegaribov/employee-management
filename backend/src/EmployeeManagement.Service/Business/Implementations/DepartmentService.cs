using AutoMapper;
using EmployeeManagement.Core.Common;
using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Exceptions;
using EmployeeManagement.Core.Filters;
using EmployeeManagement.Core.Filters.Pagination;
using EmployeeManagement.DataAccess.UnitOfWork.Abstracts;
using EmployeeManagement.Service.Business.Abstracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public DepartmentService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentForCollectionDTO>> GetAllAsync(DepartmentsQueryParams queryParams)
        {
            string[] searchablePropertyNames = { "Name" };

            var departmentPaginator = await _unitOfWork.Departments
                .GetAllSearchedPaginatedSortedAsync(queryParams.Query, queryParams.Sort, queryParams.Page, queryParams.PageSize, searchablePropertyNames);

            _httpContextAccessor.HttpContext.Response.Headers.Add(HeaderNames.XPagination, departmentPaginator.PaginationInfo);

            return _mapper.Map<IEnumerable<DepartmentForCollectionDTO>>(departmentPaginator.Records);
        }

        public async Task<DepartmentDetailsDTO> GetDetailsAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            return _mapper.Map<DepartmentDetailsDTO>(department);
        }

        public async Task<CreateDepartmentResponseDTO> CreateAsync(CreateDepartmentRequestDTO departmentDTO)
        {
            var department = _mapper.Map<Department>(departmentDTO);

            await _unitOfWork.Departments.CreateAsync(department);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CreateDepartmentResponseDTO>(department);
        }

        public async Task UpdateAsync(int id, UpdateDepartmentDTO departmentDTO)
        {
            var department = await _unitOfWork.Departments.GetAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            department = _mapper.Map<UpdateDepartmentDTO, Department>(departmentDTO, department);

            _unitOfWork.Departments.Update(department);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            _unitOfWork.Departments.Delete(department);
            await _unitOfWork.CommitAsync();
        }

        #region Department employees

        public async Task<IEnumerable<EmployeeForCollectionDTO>> GetEmployeesAsync(int departmentId, QueryParams queryParams)
        {
            string[] searchablePropertyNames = { "Name", "Surname" };

            var employeePaginator = await _unitOfWork.Employees
                .GetAllSearchedPaginatedSortedAsync(queryParams.Query, queryParams.Sort, queryParams.Page, queryParams.PageSize, searchablePropertyNames, e => e.DepartmentId == departmentId);

            _httpContextAccessor.HttpContext.Response.Headers.Add(HeaderNames.XPagination, employeePaginator.PaginationInfo);

            return _mapper.Map<IEnumerable<EmployeeForCollectionDTO>>(employeePaginator.Records);
        }

        public async Task<EmployeeDetailsResponseDTO> GetEmployeeDetailsAsync(int departmentId, int employeeId)
        {
            var employee = await _unitOfWork.Employees.GetSingleOrDefaultAsync(e => e.Id == employeeId && e.DepartmentId == departmentId);
            if (employee is null) throw new NotFoundException($"Employee not found with id {employeeId}, and department id {departmentId}");

            return _mapper.Map<EmployeeDetailsResponseDTO>(employee);
        }

        #endregion
    }
}
