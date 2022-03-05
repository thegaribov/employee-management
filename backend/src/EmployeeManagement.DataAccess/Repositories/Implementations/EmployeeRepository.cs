using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Searching;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.DataAccess.Repositories.Abstracts;
using EmployeeManagement.DataAccess.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Repositories.Implementations
{
    public class EmployeeRepository : EFBaseRepository<Employee>, IEmployeeRepository
    {
        private readonly EmployeeManagementContext _context;

        public EmployeeRepository(EmployeeManagementContext context)
            : base(context)
        {
            _context = context;
        }

        public async virtual Task<List<Employee>> GetAllSearchedAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return await GetAllAsync();
            }

            var propertyNames = typeof(Employee)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).FirstOrDefault() != null)
                .Select(x => x.Name);

            var result = string.Join(" + \" \"  + ", propertyNames);

            var expressionList = new List<string>();
            var queryList = query.Split(" ");

            for (int i = 0; i < queryList.Length; i++)
            {
                expressionList.Add($"({result}).Contains(@{i})");
            }

            var expression = string.Join(" and ", expressionList);


            return await _context.Employees.Where(expression, query.Split(" ")).ToListAsync();
        }
    }


}
