using EmployeeManagement.Core.Pagination.Shared;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters.Base
{

    public class QueryParams
    {
        /// <summary>
        /// Page number to request
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Page size to request
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Search query
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Sort query
        /// </summary>
        public string Sort { get; set; }
    }

    public class QueryParamsValidator : AbstractValidator<QueryParams>
    {
        public QueryParamsValidator()
        {
            IntegrateRules();
        }


        private void IntegrateRules()
        {
            #region PageSize

            RuleFor(queryParams => queryParams.PageSize)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .Must(qp => BasePaginator.AllowedPageSizes.Contains(qp.Value))
                .WithMessage($"Page sizes can be : {string.Join(", ", BasePaginator.AllowedPageSizes)}")
                .When(qp => qp.PageSize != null);

            RuleFor(qp => qp.Page)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .When(qp => qp.Page != null);

            #endregion
        }
    }
}
