using EmployeeManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Persistance.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            #region Name

            builder
                .Property(e => e.Name)
                .IsRequired();

            #endregion

            #region Surname

            builder
                .Property(e => e.Surname)
                .IsRequired();

            #endregion

            #region BirthDate

            builder
                .Property(e => e.BirthDate)
                .IsRequired();

            #endregion

            #region Department

            builder
               .HasOne<Department>(employee => employee.Department)
               .WithMany(department => department.Employees)
               .HasForeignKey(employee => employee.DepartmentId)
               .OnDelete(DeleteBehavior.NoAction);

            #endregion

            builder
                .ToTable("Employees");
        }
    }
}
