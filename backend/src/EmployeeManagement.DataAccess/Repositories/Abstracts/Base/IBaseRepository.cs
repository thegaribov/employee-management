﻿using EmployeeManagement.Core.Common;
using EmployeeManagement.Core.Pagination.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Abstracts.Base
{
    public interface IBaseRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null);
        Task<Paginator<TEntity>> GetAllSearchedPaginatedSortedAsync(string query, string sort, int? page, int? pageSize, Expression<Func<TEntity, bool>> expression = null);
        Task<List<TEntity>> GetAllSearchedAsync(string query, Expression<Func<TEntity, bool>> expression = null);
        Task<Paginator<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> expression = null);
        Task<List<TEntity>> GetAllSortedAsync(string query, Expression<Func<TEntity, bool>> expression = null);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetAsync(object id);
        Task CreateAsync(TEntity data);
        Task UpdateAsync(TEntity data);
        Task DeleteAsync(TEntity data);
    }
}
