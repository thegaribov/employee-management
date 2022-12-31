using AutoMapper;
using EmployeeManagement.Core.Common;
using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Exceptions;
using EmployeeManagement.Core.Filters;
using EmployeeManagement.Core.Filters.Pagination;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.Service.Business.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly EmployeeManagementContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public DepartmentService(
            EmployeeManagementContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            ICacheService cacheService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentForCollectionDTO>> GetAllAsync(DepartmentsQueryParams queryParams)
        {
            //string[] searchablePropertyNames = { "name" };

            //var departmentPaginator = await _unitOfWork.Departments
            //    .GetAllSearchedFilteredSortedPaginatedAsync(queryParams.Search, queryParams.Filter, queryParams.Sort, queryParams.Page, queryParams.PageSize, searchablePropertyNames);

            //_httpContextAccessor.HttpContext.Response.Headers.Add(HeaderNames.XPagination, departmentPaginator.GetPaginationInfo());

            //return _mapper.Map<IEnumerable<DepartmentForCollectionDTO>>(departmentPaginator.Records);

            throw new NotImplementedException();
        }

        public async Task<DepartmentDetailsDTO> GetDetailsAsync(int id)
        {
            var department = await _dbContext.Departments.FindAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            return _mapper.Map<DepartmentDetailsDTO>(department);
        }

        public async Task<CreateDepartmentResponseDTO> CreateAsync(CreateDepartmentRequestDTO departmentDTO)
        {
            var department = _mapper.Map<Department>(departmentDTO);

            await _dbContext.Departments.AddAsync(department);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CreateDepartmentResponseDTO>(department);
        }

        public async Task UpdateAsync(int id, UpdateDepartmentDTO departmentDTO)
        {
            var department = await _dbContext.Departments.FindAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            department = _mapper.Map<UpdateDepartmentDTO, Department>(departmentDTO, department);

            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _dbContext.Departments.FindAsync(id);
            if (department is null) throw new NotFoundException($"Department is not found with id {id}");

            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();
        }

        #region Department employees

        public async Task<IEnumerable<EmployeeForCollectionDTO>> GetEmployeesAsync(int departmentId, QueryParams queryParams)
        {
            //string[] searchablePropertyNames = { "Name", "Surname" };

            //var employeePaginator = await _unitOfWork.Employees
            //    .GetAllSearchedFilteredSortedPaginatedAsync(queryParams.Search, queryParams.Filter, queryParams.Sort, queryParams.Page, queryParams.PageSize, searchablePropertyNames, e => e.DepartmentId == departmentId);

            //_httpContextAccessor.HttpContext.Response.Headers.Add(HeaderNames.XPagination, employeePaginator.GetPaginationInfo());

            //return _mapper.Map<IEnumerable<EmployeeForCollectionDTO>>(employeePaginator.Records);

            throw new NotImplementedException();
        }

        public async Task<EmployeeDetailsResponseDTO> GetEmployeeDetailsAsync(int departmentId, int employeeId)
        {
            var employee = await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == employeeId && e.DepartmentId == departmentId);
            if (employee is null) throw new NotFoundException($"Employee not found with id {employeeId}, and department id {departmentId}");

            return _mapper.Map<EmployeeDetailsResponseDTO>(employee);
        }

        #endregion
    }
}
