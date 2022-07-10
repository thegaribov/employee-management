using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using EmployeeManagement.Core.Common;

namespace EmployeeManagement.Core.Filters.Pagination
{
    public class Page<TEntity> : BasePage
        where TEntity : class, new()
    {
        [JsonIgnore]
        public IQueryable<TEntity> QuerySet { get; set; }

        [JsonIgnore]
        public IEnumerable<TEntity> Data { get; set; }

        public Page(IQueryable<TEntity> query, int page, int pageSize)
            : base(page, pageSize, query.Count())
        {
            var skipCount = CalculateSkipCount(CurrentPage, PageSize);
            QuerySet = query.Skip(skipCount).Take(PageSize);
        }

        public int CalculateSkipCount(int currentPage, int pageSize)
        {
            return (currentPage - 1) * pageSize;
        }
    }
}
