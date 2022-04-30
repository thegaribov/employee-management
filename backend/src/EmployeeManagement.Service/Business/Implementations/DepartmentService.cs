using AutoMapper;
using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Exceptions;
using EmployeeManagement.Core.Filters.Base;
using EmployeeManagement.Core.Pagination.Shared;
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

        public async Task<IEnumerable<DepartmentForCollectionDTO>> GetAllAsync(QueryParams queryParams)
        {
            var departments = await _unitOfWork.Departments.GetAllSearchedPaginatedSortedAsync(queryParams.Query, queryParams.Sort, queryParams.Page, queryParams.PageSize);

            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", departments.ToJson());

            return _mapper.Map<IEnumerable<DepartmentForCollectionDTO>>(departments.Data);
        }

        public async Task<DepartmentDetailsDTO> GetDetailsAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            return _mapper.Map<DepartmentDetailsDTO>(department);
        }

        public async Task<DepartmentDetailsDTO> CreateAsync(CreateDepartmentDTO departmentDTO)
        {
            var department = _mapper.Map<CreateDepartmentDTO, Department>(departmentDTO);

            await _unitOfWork.Departments.CreateAsync(department);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<DepartmentDetailsDTO>(department);
        }

        public async Task UpdateAsync(int id, UpdateDepartmentDTO departmentDTO)
        {
            var department = await _unitOfWork.Departments.GetAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            department = _mapper.Map<UpdateDepartmentDTO, Department>(departmentDTO, department);

            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            await _unitOfWork.Departments.DeleteAsync(department);
            await _unitOfWork.CommitAsync();
        }

        #region Employees

        public async Task<IEnumerable<EmployeeForCollectionDTO>> GetDepartmentEmployeesAsync(int departmentId, QueryParams queryParams)
        {
            var employees = await _unitOfWork.Employees
                .GetAllSearchedPaginatedSortedAsync(queryParams.Query, queryParams.Sort, queryParams.Page, queryParams.PageSize, e => e.DepartmentId == departmentId);

            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", employees.ToJson());

            return _mapper.Map<IEnumerable<EmployeeForCollectionDTO>>(employees.Data);
        }

        public async Task<DepartmentEmployeeDetailsDTO> GetDepartmentEmployeeDetailsAsync(int departmentId, int employeeId)
        {
            var employee = await _unitOfWork.Employees.GetSingleOrDefaultAsync(e => e.Id == employeeId && e.DepartmentId == departmentId);
            if (employee is null) throw new NotFoundException($"Employee not found with id {employeeId}, and department id {departmentId}");

            return _mapper.Map<DepartmentEmployeeDetailsDTO>(employee);
        }

        #endregion
    }
}
