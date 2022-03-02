using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Employee
{
    public class CreateEmployeeDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public int DepartmentId { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class CreateEmployeeDTOValidator : AbstractValidator<CreateEmployeeDTO>
    {
        private string NOT_NULL_MESSAGE { get; set; } = "Cannot be empty";
        private string NOT_EMPTY_MESSAGE { get; set; } = "Cannot be empty";

        public CreateEmployeeDTOValidator()
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
