using EmployeeManagement.DataAccess.Persistance.Contexts;
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


       
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
