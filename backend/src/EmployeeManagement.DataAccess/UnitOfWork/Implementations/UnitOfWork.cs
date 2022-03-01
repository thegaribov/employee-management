using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.DataAccess.Repositories.Abstracts;
using EmployeeManagement.DataAccess.Repositories.Implementations;
using EmployeeManagement.DataAccess.UnitOfWork.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeManagementContext _context;

        public UnitOfWork(EmployeeManagementContext context)
        {
            _context = context;
        }

        private IEmployeeRepository employee;
        public IEmployeeRepository Employees => employee ??= new EmployeeRepository(_context);


        private IDepartmentRepository department;
        public IDepartmentRepository Departments => department ??= new DepartmentRepository(_context);


        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
