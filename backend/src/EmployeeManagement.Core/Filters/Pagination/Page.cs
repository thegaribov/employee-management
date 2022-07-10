using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EmployeeManagement.Core.Common;
using Newtonsoft.Json.Serialization;

namespace EmployeeManagement.Core.Filters.Pagination
{
    public class Page<TEntity> where TEntity : class, new()
    {
        [JsonIgnore]
        public IQueryable<TEntity> QuerySet { get; set; }

        [JsonIgnore]
        public IEnumerable<TEntity> Records { get; set; }


        public bool IsPaginable { get; private set; }

        public int PageSize { get; private set; }

        public int CurrentPage { get; private set; }
        public int FirstPage { get; private set; } = 1;
        public int LastPage { get; private set; }

        public bool HasNextPage { get; private set; }
        public int? NextPage { get; private set; }
        public bool HasPreviousPage { get; private set; }
        public int? PreviousPage { get; private set; }

        public int TotalRecords { get; private set; }


        public Page(IQueryable<TEntity> query, int? page, int? pageSize)
        {
            PageSize = pageSize ?? 10;
            TotalRecords = query.Count();

            LastPage = GetLastPage(TotalRecords, PageSize);

            IsPaginable = CheckIsPaginable(FirstPage, LastPage);
            CurrentPage = GetCurrentPage(page ?? 1, LastPage);

            HasNextPage = CheckHasNextPage(CurrentPage, LastPage);
            NextPage = GetNextPage(CurrentPage, HasNextPage);

            HasPreviousPage = CheckHasPreviousPage(CurrentPage, FirstPage);
            PreviousPage = GetPreviousPage(CurrentPage, HasPreviousPage);

            var skipCount = CalculateSkipCount(CurrentPage, PageSize);
            QuerySet = query.Skip(skipCount).Take(PageSize);
        }

        private bool CheckIsPaginable(int firstPage, int lastPage)
        {
            return lastPage - firstPage > 0;
        }
        private int GetCurrentPage(int requestedPage, int lastPage)
        {
            return requestedPage > lastPage ? lastPage : requestedPage;
        }
        private int GetLastPage(int recordsCount, int pageSize)
        {
            if (recordsCount > 0)
            {
                var pageCount = (double)recordsCount / pageSize;
                return (int)Math.Ceiling(pageCount);
            }

            return 1;
        }
        private bool CheckHasNextPage(int currentPage, int lastPage)
        {
            return lastPage - currentPage > 0;
        }
        private int? GetNextPage(int currentPage, bool hasNextPage)
        {
            return hasNextPage ? currentPage + 1 : null;
        }
        private bool CheckHasPreviousPage(int currentPage, int firstPage)
        {
            return currentPage - firstPage > 0;
        }
        private int? GetPreviousPage(int currentPage, bool hasPreviousPage)
        {
            return hasPreviousPage ? currentPage - 1 : null;
        }
        private int CalculateSkipCount(int currentPage, int pageSize)
        {
            return (currentPage - 1) * pageSize;
        }

        public string PaginationInfo { get => JsonSerializer.Serialize(this); }
    }
}
