using EmployeeManagement.Core.Common;
using EmployeeManagement.Core.Filters.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Abstracts.Base
{
    public interface IBaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null);
        Task<Page<TEntity>> GetAllSearchedFilteredSortedPaginatedAsync(string search, string filter, string sort, int? page, int? pageSize, string[] searchablePropertyNames, Expression<Func<TEntity, bool>> expression = null);
        Task<List<TEntity>> GetAllSearchedAsync(string query, string[] searchablePropertyNames, Expression<Func<TEntity, bool>> expression = null);
        Task<Page<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> expression = null);
        Task<List<TEntity>> GetAllSortedAsync(string query, Expression<Func<TEntity, bool>> expression = null);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetAsync(object id);
        Task CreateAsync(TEntity data);
        void Update(TEntity data);
        void Delete(TEntity data);
    }
}
