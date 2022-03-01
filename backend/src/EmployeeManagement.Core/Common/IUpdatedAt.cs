using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Common
{
    public interface IUpdatedAt
    {
        public DateTime UpdatedAt { get; set; }
    }
}
