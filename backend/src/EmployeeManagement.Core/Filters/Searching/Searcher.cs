using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters.Searching
{
    public class Searcher<TEntity, TKey>
         where TEntity : class, IEntity<TKey>, new()
    {
        public string GetQuery(string query, string[] searchablePropertyNames)
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

                return string.Join(" and ", expressions);
            }


            return String.Empty;
        }
    }
}
