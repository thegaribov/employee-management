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
        /// Page number to request
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
}
