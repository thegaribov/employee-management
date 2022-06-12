using EmployeeManagement.Core.Entities;
using EmployeeManagement.DataAccess.Repositories.Abstracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Abstracts
{
    public interface IDepartmentRepository : IBaseRepository<Department, int>
    {

    }
}
