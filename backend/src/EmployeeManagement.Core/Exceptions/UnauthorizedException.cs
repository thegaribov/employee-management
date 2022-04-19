using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Exceptions
{
    public class UnauthorizedException : Exception
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
