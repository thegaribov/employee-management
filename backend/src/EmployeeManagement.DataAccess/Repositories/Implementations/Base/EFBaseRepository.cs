﻿using EmployeeManagement.Core.Common;
using EmployeeManagement.Core.Filters.Pagination;
using EmployeeManagement.Core.Filters.Searching;
using EmployeeManagement.Core.Filters.Sorting;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.DataAccess.Repositories.Abstracts.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Implementations.Base
{
    public abstract class EFBaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        protected readonly EmployeeManagementContext _dbContext;
        protected readonly Sorter _sorter = new Sorter();
        protected readonly Searcher<TEntity, TKey> _searcher = new Searcher<TEntity, TKey>();
        private readonly DbSet<TEntity> _dbTable;

        public EFBaseRepository(EmployeeManagementContext dbContext)
        {
            _dbContext = dbContext;
            _dbTable = _dbContext.Set<TEntity>();
        }

        public async virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            if (expression != null)
                querySet = querySet.Where(expression);

            return await querySet.ToListAsync();
        }

        public async virtual Task<Page<TEntity>> GetAllSearchedPaginatedSortedAsync(string query, string sort, int? page, int? pageSize, string[] searchablePropertyNames, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            if (expression is not null)
                querySet = querySet.Where(expression);

            //Search part
            if (!string.IsNullOrEmpty(query))
            {
                var sortQuery = _searcher.GetQuery(query, searchablePropertyNames);

                if (!string.IsNullOrEmpty(sortQuery))
                {
                    querySet = querySet.Where(sortQuery, query.Split(" "));
                }
            }

            //Pagination part
            var paginator = new Page<TEntity>(querySet.OrderBy(o => o.Id), page, pageSize);

            //Sorting part
            if (!string.IsNullOrEmpty(sort))
            {
                var sortQuery = _sorter.GetQuery(sort);

                if (!string.IsNullOrEmpty(sortQuery))
                {
                    paginator.QuerySet = paginator.QuerySet.OrderBy(sortQuery);
                }
            }

            paginator.Data = await paginator.QuerySet.ToListAsync();

            return paginator;
        }

        public async virtual Task<List<TEntity>> GetAllSortedAsync(string query, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            //Predicate part
            if (expression != null)
                querySet = querySet.Where(expression);

            var sortQuery = _sorter.GetQuery(query);

            if (!string.IsNullOrEmpty(sortQuery))
            {
                return await querySet.OrderBy(sortQuery).ToListAsync();
            }

            return await GetAllAsync();
        }

        public async virtual Task<List<TEntity>> GetAllSearchedAsync(string query, string[] searchablePropertyNames, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            //Predicate part
            if (expression != null)
                querySet = querySet.Where(expression);

            var sortQuery = _searcher.GetQuery(query, searchablePropertyNames);

            if (!string.IsNullOrEmpty(sortQuery) && !string.IsNullOrEmpty(query))
            {
                return await querySet.Where(sortQuery, query.Split(" ")).ToListAsync();
            }

            return await GetAllAsync(expression);
        }

        public async virtual Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbTable.FirstOrDefaultAsync(expression);
        }

        public async virtual Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbTable.SingleOrDefaultAsync(expression);
        }

        public async virtual Task<TEntity> GetAsync(object id)
        {
            return await _dbTable.FindAsync(id);
        }

        public async virtual Task CreateAsync(TEntity data)
        {
            await _dbTable.AddAsync(data);
        }

        public virtual void Update(TEntity data)
        {
            _dbTable.Attach(data);
            _dbContext.Entry(data).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity data)
        {
            _dbTable.Remove(data);
        }

        public async Task<Page<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            var paginator = new Page<TEntity>(querySet, page, pageSize);
            paginator.Data = await paginator.QuerySet.ToListAsync();

            return paginator;
        }
    }
}
