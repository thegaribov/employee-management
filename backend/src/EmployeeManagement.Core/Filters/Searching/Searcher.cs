using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters.Searching
{
    public class Searcher<TEntity, TKey>
         where TEntity : class, IEntity<TKey>, new()
    {
        public IQueryable<TEntity> GetQuery(IQueryable<TEntity> querySet, string query, string[] searchablePropertyNames)
        {
            if (query is not null && searchablePropertyNames.Any())
            {
                var expressions = new List<string>();
                var concantenatePropertiesExpression = string.Join(" + \" \"  + ", searchablePropertyNames);
                var queries = query.Split(" ");

                for (int i = 0; i < queries.Length; i++)
                {
                    expressions.Add($"({concantenatePropertiesExpression}).Contains(@{i})");
                }

                string sortQuery = string.Join(" and ", expressions);

                if (!string.IsNullOrEmpty(sortQuery))
                {
                    return querySet.Where(sortQuery, query.Split(" "));
                }
            }

            return querySet;
        }
    }
}
