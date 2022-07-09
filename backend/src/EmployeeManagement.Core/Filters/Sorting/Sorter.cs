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
        public List<string> GetSortablePropertyNames()
        {
            return typeof(TEntity)
               .GetProperties()
               .Where(p => p.GetCustomAttributes(typeof(SortableAttribute), true).FirstOrDefault() != null)
               .Select(x => x.Name.ToLowerInvariant())
               .ToList();
        }

        public string GetQuery(string query)
        {
            var sortablePropertyNames = GetSortablePropertyNames();
            var sortQueries = new List<string>();

            if (query != null && sortablePropertyNames.Any())
            {
                var queries = query.ToLowerInvariant().Replace(" ", "").Split(",");
               

                var pattern = $"^({string.Join("|", sortablePropertyNames)})_(asc|desc)$";

                Regex regex = new Regex(pattern);


                foreach (var singleQuery in queries)
                {
                    if (regex.IsMatch(singleQuery))
                    {
                        sortQueries.Add(singleQuery.Replace("_", " "));
                    }
                }
            }


            return String.Join(",", sortQueries);
        }
        


    }
}
