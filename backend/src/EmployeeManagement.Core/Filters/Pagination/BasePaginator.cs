using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Filters.Pagination
{
    public abstract class BasePaginator
    {
        public static int[] AllowedPageSizes { get => new int[] { 5, 10, 15, 25 }; }

        public bool IsPaginable { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
        public int FirstPage { get; set; } = 1;
        public int LastPage { get; set; }

        public bool HasNextPage { get; set; }
        public int? NextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public int? PreviousPage { get; set; }

        public int TotalRecords { get; set; }

        public BasePaginator(int page, int pageSize, int recordsCount)
        {
            PageSize = pageSize;
            TotalRecords = recordsCount;

            FirstPage = GetFirstPage();
            LastPage = GetLastPage(recordsCount, PageSize);

            IsPaginable = CheckIsPaginable(FirstPage, LastPage);
            CurrentPage = GetCurrentPage(page, LastPage);

            HasNextPage = CheckHasNextPage(CurrentPage, LastPage);
            NextPage = GetNextPage(CurrentPage, HasNextPage);

            HasPreviousPage = CheckHasPreviousPage(CurrentPage, FirstPage);
            PreviousPage = GetPreviousPage(CurrentPage, HasPreviousPage);
        }

        public bool CheckIsPaginable(int firstPage, int lastPage)
        {
            return lastPage - firstPage > 0;
        }
        public int GetCurrentPage(int requestedPage, int lastPage)
        {
            return requestedPage > lastPage ? lastPage : requestedPage;
        }
        public int GetFirstPage()
        {
            return 1;
        }
        public int GetLastPage(int recordsCount, int pageSize)
        {
            if (recordsCount > 0)
            {
                var pageCount = (double)recordsCount / pageSize;
                return (int)Math.Ceiling(pageCount);
            }

            return 1;
        }
        public bool CheckHasNextPage(int currentPage, int lastPage)
        {
            return lastPage - currentPage > 0;
        }
        public int? GetNextPage(int currentPage, bool hastNextPage)
        {
            return hastNextPage ? currentPage + 1 : null;
        }
        public bool CheckHasPreviousPage(int currentPage, int firstPage)
        {
            return currentPage - firstPage > 0;
        }
        public int? GetPreviousPage(int currentPage, bool hastPreviousPage)
        {
            return hastPreviousPage ? currentPage - 1 : null;
        }

        public string ToJson()
        {
            var serializeSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(this, serializeSettings);
        }
    }
}
