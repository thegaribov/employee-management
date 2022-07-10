using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Filters.Searching;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.DataAccess.Repositories.Abstracts;
using EmployeeManagement.DataAccess.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Implementations
{
    public class EmployeeRepository : EFBaseRepository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeManagementContext context)
            : base(context)
        {

        }
    }


}
