using EmployeeManagement.Core.Filters;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.DTOs.v1.Department
{
    public class DepartmentsQueryParams : QueryParams { }

    public class DepartmentsQueryParamsValidator : AbstractValidator<DepartmentsQueryParams>
    {
        private readonly string[] _sortablePropertyNames;
        private readonly int[] _allowedPageSizes = { 5, 10, 15, 25 };
        private readonly Regex _regex;

        public DepartmentsQueryParamsValidator()
        {
            var department = GetDepartment();
            _sortablePropertyNames = new string[] { nameof(department.Name), nameof(department.CreatedAt) };
            _regex = GetSortingRegex();

            IntegrateRules();
        }

        private void IntegrateRules()
        {
            RuleFor(qp => qp.PageSize)
                .Cascade(CascadeMode.Stop)
                .Must(qp => _allowedPageSizes.Contains(qp.Value))
                .WithMessage($"Page sizes can be : {string.Join(", ", _allowedPageSizes)}")
                .When(qp => qp.PageSize is not null);

            RuleFor(qp => qp.Page)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .When(qp => qp.Page is not null);

            RuleFor(qp => qp.Sort)
                .Cascade(CascadeMode.Stop)
                .Must(s => s.Split(',').All(pr => _regex.IsMatch(pr)))
                .WithMessage($"Allowed sorting columns are : {string.Join(", ", _sortablePropertyNames)}")
                .When(qp => qp.Sort is not null);
        }

        private Entities.Department GetDepartment()
        {
            return new();
        }

        private Regex GetSortingRegex()
        {
            return new($"^(?i)({string.Join("|", _sortablePropertyNames)})_(asc|desc)$");
        }
    }
}
