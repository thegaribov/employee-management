using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Department
{
    public class EmployeeDetailsResponseDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
