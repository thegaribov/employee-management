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

        public async virtual Task<List<TEntity>> GetAllAsync()
        {
            return await _dbTable.ToListAsync();
        }

        public async virtual Task<Paginator<TEntity>> GetAllSearchedPaginatedSortedAsync(string query, string sort, int? page, int? pageSize)
        {
            var querySet = _dbTable.AsQueryable();

            //Search part
            if (!string.IsNullOrEmpty(query))
            {
                var searcher = new Searcher<TEntity>();
                var sortQuery = searcher.GetQuery(query);

                querySet = querySet.Where(sortQuery, query.Split(" "));
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

            var resultQuery = paginator.QuerySet.ToQueryString();

            paginator.Data = await paginator.QuerySet.ToListAsync();
     
            return paginator;
        }

        public async virtual Task<Paginator<TEntity>> GetAllPaginatedAsync(int page, int pageSize)
        {
            var paginator = new Paginator<TEntity>(_dbTable.AsQueryable(), page, pageSize);
            paginator.Data = await paginator.QuerySet.ToListAsync();

            return paginator;
        }

        public async virtual Task<List<TEntity>> GetAllSortedAsync(string query)
        {
            var sorter = new Sorter<TEntity>();
            var sortQuery = sorter.GetQuery(query);

            if (!string.IsNullOrEmpty(sortQuery))
            {
                return await _dbTable.AsQueryable().OrderBy(sortQuery).ToListAsync();
            }

            return await GetAllAsync();
        }

        public async virtual Task<List<TEntity>> GetAllSearchedAsync(string query)
        {
            var searcher = new Searcher<TEntity>();
            var sortQuery = searcher.GetQuery(query);

            if (!string.IsNullOrEmpty(sortQuery) && !string.IsNullOrEmpty(query))
            {
                return await _dbTable.AsQueryable().Where(sortQuery, query.Split(" ")).ToListAsync();
            }

            return await GetAllAsync();
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
