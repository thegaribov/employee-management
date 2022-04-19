using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key) 
            : base($"{name} ({key}) not found in database")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
