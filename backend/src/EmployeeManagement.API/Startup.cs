using AutoMapper;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.DataAccess.UnitOfWork.Abstracts;
using EmployeeManagement.DataAccess.UnitOfWork.Implementations;
using EmployeeManagement.Service.Business.Abstracts;
using EmployeeManagement.Service.Business.Implementations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmployeeManagement.API
{
    public class Startup
    {
        public bool IsProduction { get; set; }
        public bool IsStaging { get; set; }
        public bool IsDevelopment { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IsProduction = environment == Environments.Production;
            IsStaging = environment == Environments.Staging;
            IsDevelopment = environment == Environments.Development;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            #region Database context

            services.AddDbContext<EmployeeManagementContext>(option =>
            {
                var connectionString = string.Empty;

                if (IsDevelopment)
                {
                    //Environment.GetEnvironmentVariable("CONNECTION_STRING_NAME")
                    //Production

                    connectionString = Configuration
                           .GetConnectionString(Environment.GetEnvironmentVariable("CONNECTION_STRING_NAME"));
                }
                else
                {
                    string serverName = Environment.GetEnvironmentVariable("SQL_SERVER_NAME");
                    string database = Environment.GetEnvironmentVariable("SQL_DATABASE");
                    string user = Environment.GetEnvironmentVariable("SQL_USER");
                    string password = Environment.GetEnvironmentVariable("SQL_PASSWORD");

                    connectionString = @$"Server={serverName};Database={database};User={user};Password={password};";
                }

                option.UseSqlServer(connectionString, x => x.MigrationsAssembly("EmployeeManagement.DataAccess"));
            });

            #endregion

            #region Routing configurations

            //Lowercase Routing
            services.AddRouting(options => options.LowercaseUrls = true);

            #endregion

            #region Services

            //Unit of work
            services.AddTransient<IUnitOfWork, UnitOfWork>();


            //Business
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IDepartmentService, DepartmentService>();


            //Mappers
            services.AddAutoMapper(Assembly.Load("EmployeeManagement.API"));

            #endregion
            

            #region FluentValidation

            var dtoValidationsAssembly = "EmployeeManagement.Core";

            services.AddFluentValidation(fv => 
                    fv.RegisterValidatorsFromAssembly(Assembly.Load(dtoValidationsAssembly)));

            #endregion

            #region Data protection keys

            services.AddDataProtection()
                .PersistKeysToDbContext<EmployeeManagementContext>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
