using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters.Base
{
    public class QueryParams
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string SortQuery { get; set; }
        public string SearchQuery { get; set; }
    }
}
