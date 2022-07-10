using EmployeeManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
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
                var propertyNames = AppendToStringStatementToNumberTypes(searchablePropertyNames);
                var expressions = new List<string>();
                var concantenatePropertiesExpression = string.Join(" + \" \"  + ", propertyNames);
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

        private List<string> AppendToStringStatementToNumberTypes(string[] searchablePropertyNames)
        {
            List<string> propertyNames = new List<string>();

            for (int i = 0; i < searchablePropertyNames.Length; i++)
            {
                PropertyInfo propertyInfo = typeof(TEntity).GetProperty(searchablePropertyNames[i], BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                if (propertyInfo is not null)
                {
                    var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
                    switch (typeCode)
                    {
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Decimal:
                        case TypeCode.Double:
                        case TypeCode.Single:
                            propertyNames.Add(searchablePropertyNames[i] + ".ToString()");
                            break;
                        default:
                            propertyNames.Add(searchablePropertyNames[i]);
                            break;
                    }
                }
            }

            return propertyNames;
        }
    
    }
}
