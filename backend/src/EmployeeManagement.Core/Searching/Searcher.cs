using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Searching
{
    public class Searcher<TEntity>
         where TEntity : class, IEntity, new()
    {
        public IEnumerable<string> GetSearchablePropertyNames()
        {
            return typeof(TEntity)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).FirstOrDefault() != null)
                .Select(x => x.Name);
        }

        public string GetQuery(string query)
        {
            var searchablePropertyNames = GetSearchablePropertyNames();

            if (query != null && searchablePropertyNames.Any())
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
