using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Pagination.Shared
{
    public class Paginator<T> : BasePaginator
    {
        [JsonIgnore]
        public IQueryable<T> Query { get; set; }

        [JsonIgnore]
        public IEnumerable<T> Data { get; set; }

        public Paginator(IQueryable<T> query, int page, int pageSize)
            :base(page, pageSize, query.Count())
        {
            var skipCount = CalculateSkipCount(CurrentPage, PageSize);
            Query = query.Skip(skipCount).Take(PageSize);
        }

        public int CalculateSkipCount(int currentPage, int pageSize)
        {
            return (currentPage - 1) * pageSize;
        }
    }
}
