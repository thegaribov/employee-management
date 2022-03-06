using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Employee
{
    public class UpdateEmployeeDTO
    {
        /// <summary>
        /// Employee name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Employee surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Related department id
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Employee birth date
        /// </summary>
        public DateTime BirthDate { get; set; }
    }

    public class UpdateEmployeeDTOValidator : AbstractValidator<UpdateEmployeeDTO>
    {
        private string NOT_NULL_MESSAGE { get; set; } = "Cannot be empty";
        private string NOT_EMPTY_MESSAGE { get; set; } = "Cannot be empty";

        public UpdateEmployeeDTOValidator()
        {
            IntegrateRules();
        }

        
        private void IntegrateRules()
        {
            #region Name

            RuleFor(employee => employee.Name)
                .Cascade(CascadeMode.Stop)

                .NotNull()
                .WithMessage(NOT_NULL_MESSAGE)

                .NotEmpty()
                .WithMessage(NOT_EMPTY_MESSAGE);

            #endregion

            #region Surname

            RuleFor(employee => employee.Surname)
                .Cascade(CascadeMode.Stop)

                .NotNull()
                .WithMessage(NOT_NULL_MESSAGE)

                .NotEmpty()
                .WithMessage(NOT_EMPTY_MESSAGE);

            #endregion

            #region DepartmentId

            RuleFor(employee => employee.DepartmentId)
                .Cascade(CascadeMode.Stop)

                .NotNull()
                .WithMessage(NOT_NULL_MESSAGE)

                .NotEmpty()
                .WithMessage(NOT_EMPTY_MESSAGE);

            #endregion

            #region BirthDate

            RuleFor(employee => employee.BirthDate)
                .Cascade(CascadeMode.Stop)

                .NotNull()
                .WithMessage(NOT_NULL_MESSAGE)

                .NotEmpty()
                .WithMessage(NOT_EMPTY_MESSAGE);

            #endregion
        }
    }
}
