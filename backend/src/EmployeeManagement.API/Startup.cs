using EmployeeManagement.DataAccess.Persistance.Contexts;
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

            //unitOfWork
            //services.AddTransient<IUnitOfWork, UnitOfWork>();

           
            #endregion


            #region FluentValidation

            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

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
