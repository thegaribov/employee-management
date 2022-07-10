using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters.Sorting
{
    public class Sorter<TEntity, TKey>
         where TEntity : class, IEntity<TKey>, new()
    {
        public string GetQuery(string query)
        {
            return query.Replace("_", " ");
        }
    }
}
