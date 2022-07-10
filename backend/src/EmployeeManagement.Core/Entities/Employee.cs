using EmployeeManagement.Core.Common;
using EmployeeManagement.Core.Filters.Searching;
using EmployeeManagement.Core.Filters.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Entities
{
    public class Employee : IEntity<int>, ICreatedAt, IUpdatedAt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
