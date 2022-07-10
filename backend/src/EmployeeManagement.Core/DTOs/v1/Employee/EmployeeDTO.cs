using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Employee
{
    public class EmployeeDTO
    {
        /// <summary>
        /// Employee id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Employee surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Employee age
        /// </summary>
        public int Age { get; set; }
        public int MonthlyPayment { get; set; }

        /// <summary>
        /// Related department id
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Employee birth date
        /// </summary>
        public DateTime BirthDate { get; set; }


        /// <summary>
        /// Employee created date
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
