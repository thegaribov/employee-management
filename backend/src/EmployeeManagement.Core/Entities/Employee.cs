using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Entities
{
    public class Employee : IEntity, ICreatedAt, IUpdatedAt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
