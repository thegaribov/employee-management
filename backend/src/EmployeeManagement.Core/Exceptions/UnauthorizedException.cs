using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException() 
            : base("User should login to system")
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
