using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Department
{
    public class CreateDepartmentDTO
    {
        /// <summary>
        /// Department name
        /// </summary>
        public string Name { get; set; }
    }

    public class CreateDepartmentDTOValidator : AbstractValidator<CreateDepartmentDTO>
    {
        private string NOT_NULL_MESSAGE { get; set; } = "Cannot be empty";
        private string NOT_EMPTY_MESSAGE { get; set; } = "Cannot be empty";

        public CreateDepartmentDTOValidator()
        {
            IntegrateRules();
        }

        
        private void IntegrateRules()
        {
            #region Name

            RuleFor(department => department.Name)
                .Cascade(CascadeMode.Stop)

                .NotNull()
                .WithMessage(NOT_NULL_MESSAGE)

                .NotEmpty()
                .WithMessage(NOT_EMPTY_MESSAGE);

            #endregion
        }
    }
}
