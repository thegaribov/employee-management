using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Department
{
    public class UpdateDepartmentDTO
    {
        public string Name { get; set; }
    }

    public class UpdateDepartmentDTOValidator : AbstractValidator<UpdateDepartmentDTO>
    {
        private string NOT_NULL_MESSAGE { get; set; } = "Cannot be empty";
        private string NOT_EMPTY_MESSAGE { get; set; } = "Cannot be empty";

        public UpdateDepartmentDTOValidator()
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
