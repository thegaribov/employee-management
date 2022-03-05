using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Sorting
{
    public class Sorter<TEntity>
         where TEntity : class, IEntity, new()
    {
        public List<string> GetPermittedProperties()
        {
            return typeof(TEntity)
               .GetProperties()
               .Where(p => p.GetCustomAttributes(typeof(SortableAttribute), true).FirstOrDefault() != null)
               .Select(x => x.Name.ToLowerInvariant())
               .ToList();
        }

        public string GetQuery(string queryString)
        {
            var resultOrderBys = new List<string>();

            if (queryString != null)
            {
                var orderBys = queryString.ToLowerInvariant().Replace(" ", "").Split(",");
                var avaiableProperties = GetPermittedProperties();

                var pattern = $"^({string.Join("|", avaiableProperties)})_(asc|desc)$";

                Regex regex = new Regex(pattern);


                foreach (var orderBy in orderBys)
                {
                    if (regex.IsMatch(orderBy))
                    {
                        resultOrderBys.Add(orderBy.Replace("_", " "));
                    }
                }
            }


            return String.Join(",", resultOrderBys);
        }
        


    }
}
