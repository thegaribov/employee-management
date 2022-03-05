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
        public IEnumerable<string> GetPermittedProperties()
        {
            return typeof(TEntity)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).FirstOrDefault() != null)
                .Select(x => x.Name);
        }

        public string GetQuery(string queryString)
        {
            if (queryString != null)
            {
                var expressions = new List<string>();
                var avaiableProperties = GetPermittedProperties();
                var propertyExpression = string.Join(" + \" \"  + ", avaiableProperties);
                var splittedQueries = queryString.Split(" ");

                for (int i = 0; i < splittedQueries.Length; i++)
                {
                    expressions.Add($"({propertyExpression}).Contains(@{i})");
                }

                return string.Join(" and ", expressions);
            }


            return String.Empty;
        }
    }
}
