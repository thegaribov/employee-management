using EmployeeManagement.DataAccess.Repositories.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.UnitOfWork.Abstracts
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }

        Task CommitAsync();
    }
}
