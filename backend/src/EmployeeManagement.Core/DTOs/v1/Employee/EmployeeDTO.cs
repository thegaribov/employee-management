using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Employee
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public int DepartmentId { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
