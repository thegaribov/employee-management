using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.UnitOfWork.Abstracts
{
    public interface IUnitOfWork
    {
        


        Task CommitAsync();
    }
}
