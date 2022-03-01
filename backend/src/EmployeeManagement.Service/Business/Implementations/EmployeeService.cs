using EmployeeManagement.DataAccess.UnitOfWork.Abstracts;
using EmployeeManagement.Service.Business.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Core.Entities.Employee>> GetAllAsync()
        {
            return await _unitOfWork.Employees.GetAllAsync();
        }


        public async Task<Core.Entities.Employee> GetAsync(int id)
        {
            return await _unitOfWork.Employees.GetAsync(id);
        }

        public async Task CreateAsync(Core.Entities.Employee ticket)
        {
            await _unitOfWork.Employees.CreateAsync(ticket);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(Core.Entities.Employee ticket)
        {
            await _unitOfWork.Employees.UpdateAsync(ticket);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(Core.Entities.Employee ticket)
        {
            await _unitOfWork.Employees.DeleteAsync(ticket);
            await _unitOfWork.CommitAsync();
        }

    }
}
