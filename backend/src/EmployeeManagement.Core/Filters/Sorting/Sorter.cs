using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters.Sorting
{
    public class Sorter<TEntity, TKey>
         where TEntity : class, IEntity<TKey>, new()
    {
        public IQueryable<TEntity> GetQuery(IQueryable<TEntity> querySet, string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var sortQuery = query.Replace("_", " ");

                if (!string.IsNullOrEmpty(sortQuery))
                {
                    return querySet.OrderBy(sortQuery);
                }
            }

            return querySet;
        }
    }
}
