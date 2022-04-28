using EmployeeManagement.Core.Common;
using EmployeeManagement.Core.Pagination.Shared;
using EmployeeManagement.Core.Searching;
using EmployeeManagement.Core.Sorting;
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
    public abstract class EFBaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly EmployeeManagementContext _db;
        private readonly DbSet<TEntity> _dbTable;

        public EFBaseRepository(EmployeeManagementContext db)
        {
            _db = db;
            _dbTable = db.Set<TEntity>();
        }

        public async virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            if (expression != null)
                querySet = querySet.Where(expression);

            return await querySet.ToListAsync();
        }

        public async virtual Task<Paginator<TEntity>> GetAllSearchedPaginatedSortedAsync(string query, string sort, int? page, int? pageSize, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            //Predicate part
            if (expression != null)
                querySet = querySet.Where(expression);

            //Search part
            if (!string.IsNullOrEmpty(query))
            {
                var searcher = new Searcher<TEntity>();
                var sortQuery = searcher.GetQuery(query);

                if (!string.IsNullOrEmpty(sortQuery))
                {
                    querySet = querySet.Where(sortQuery, query.Split(" "));
                }
            }

            //Pagination part
            var paginator = new Paginator<TEntity>(querySet, page.GetValueOrDefault(1), pageSize.GetValueOrDefault(10));

            //Sorting part
            if (!string.IsNullOrEmpty(sort))
            {
                var sorter = new Sorter<TEntity>();
                var sortQuery = sorter.GetQuery(sort);

                if (!string.IsNullOrEmpty(sortQuery))
                {
                    paginator.QuerySet = paginator.QuerySet.OrderBy(sortQuery);
                }
            }

            paginator.Data = await paginator.QuerySet.ToListAsync();
     
            return paginator;
        }

        public async virtual Task<Paginator<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            //Predicate part
            if (expression != null)
                querySet = querySet.Where(expression);

            var paginator = new Paginator<TEntity>(querySet, page, pageSize);
            paginator.Data = await paginator.QuerySet.ToListAsync();

            return paginator;
        }

        public async virtual Task<List<TEntity>> GetAllSortedAsync(string query, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            //Predicate part
            if (expression != null)
                querySet = querySet.Where(expression);

            var sorter = new Sorter<TEntity>();
            var sortQuery = sorter.GetQuery(query);

            if (!string.IsNullOrEmpty(sortQuery))
            {
                return await querySet.OrderBy(sortQuery).ToListAsync();
            }

            return await GetAllAsync();
        }

        public async virtual Task<List<TEntity>> GetAllSearchedAsync(string query, Expression<Func<TEntity, bool>> expression = null)
        {
            var querySet = _dbTable.AsQueryable();

            //Predicate part
            if (expression != null)
                querySet = querySet.Where(expression);

            var searcher = new Searcher<TEntity>();
            var sortQuery = searcher.GetQuery(query);

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

        public async virtual Task UpdateAsync(TEntity data)
        {
            _dbTable.Attach(data);
            _db.Entry(data).State = EntityState.Modified;
        }

        public async virtual Task DeleteAsync(TEntity data)
        {
            _dbTable.Remove(data);
        }
    }
}
