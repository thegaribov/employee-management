using EmployeeManagement.Core.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Entities
{
    public class Department : IEntity, ICreatedAt, IUpdatedAt
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
