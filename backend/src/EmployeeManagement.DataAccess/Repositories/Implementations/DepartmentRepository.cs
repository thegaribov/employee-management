using EmployeeManagement.Core.Entities;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.DataAccess.Repositories.Abstracts;
using EmployeeManagement.DataAccess.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Implementations
{
    public class DepartmentRepository : EFBaseRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(EmployeeManagementContext context) 
            : base(context) 
        {
            
        }



    }
}
