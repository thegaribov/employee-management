using EmployeeManagement.Core.Filters.Pagination;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters
{

    public class QueryParams
    {
        /// <summary>
        /// Search query
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Filter query
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Sort query
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Page number to request
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Page size to request
        /// </summary>
        public int? PageSize { get; set; }
    }
}
