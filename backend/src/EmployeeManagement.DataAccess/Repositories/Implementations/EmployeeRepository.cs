using EmployeeManagement.Core.Entities;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.DataAccess.Repositories.Abstracts;
using EmployeeManagement.DataAccess.Repositories.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Implementations
{
    public class EmployeeRepository : EFBaseRepository<Employee>, IEmployeeRepository
    {
        private readonly EmployeeManagementContext _context;

        public EmployeeRepository(EmployeeManagementContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
