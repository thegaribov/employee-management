using EmployeeManagement.Core.Pagination.Shared;
using EmployeeManagement.DataAccess.UnitOfWork.Abstracts;
using EmployeeManagement.Service.Business.Abstracts;
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
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Core.Entities.Department>> GetAllAsync()
        {
            return await _unitOfWork.Departments.GetAllAsync();
        }

        public async Task<Paginator<Core.Entities.Department>> GetAllSearchedPaginatedSortedAsync(string query, string sort, int? page, int? pageSize)
        {
            return await _unitOfWork.Departments.GetAllSearchedPaginatedSortedAsync(query, sort, page, pageSize);
        }


        public async Task<Core.Entities.Department> GetAsync(int id)
        {
            return await _unitOfWork.Departments.GetAsync(id);
        }

        public async Task CreateAsync(Core.Entities.Department department)
        {
            await _unitOfWork.Departments.CreateAsync(department);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(Core.Entities.Department department)
        {
            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(Core.Entities.Department department)
        {
            await _unitOfWork.Departments.DeleteAsync(department);
            await _unitOfWork.CommitAsync();
        }

    }
}
