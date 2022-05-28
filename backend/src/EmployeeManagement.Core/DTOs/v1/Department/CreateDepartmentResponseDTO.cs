using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Department
{
    public class CreateDepartmentResponseDTO
    {
        /// <summary>
        /// Department id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Department name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Department create date
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
