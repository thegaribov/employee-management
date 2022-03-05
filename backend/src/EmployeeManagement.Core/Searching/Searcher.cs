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
        //public List<string> GetPermittedProperties()
        //{
        //    var entityType = typeof(TEntity);
        //    var sortableProperties = new List<string>();

        //    foreach (var entityProperty in entityType.GetProperties())
        //    {
        //        if (Attribute.IsDefined(entityProperty, typeof(SearchableAttribute)))
        //        {
        //            sortableProperties.Add(entityProperty.GetValue();
        //        }
        //    }

        //    return sortableProperties;
        //}
        //public string GetSortQuery(string queryString)
        //{
        //    var resultOrderBys = new List<string>();

        //    if (queryString != null)
        //    {
        //        var orderBys = queryString.ToLowerInvariant().Replace(" ", "").Split(",");
        //        var avaiableProperties = GetPermittedProperties();

        //        var pattern = $"^({string.Join("|", avaiableProperties)})_(asc|desc)$";

        //        Regex regex = new Regex(pattern);


        //        foreach (var orderBy in orderBys)
        //        {
        //            if (regex.IsMatch(orderBy))
        //            {
        //                resultOrderBys.Add(orderBy.Replace("_", " "));
        //            }
        //        }
        //    }


        //    return String.Join(",", resultOrderBys);
        //}
    }
}
