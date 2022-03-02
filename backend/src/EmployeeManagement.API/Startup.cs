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
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                )
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });



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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion

            #region Swagger UI

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Employee management",
                    Description = "Endpoints to manage employees and departments",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Mahmood Garibov",
                        Email = "mahmood.garibov@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/mahmood-garibov-3a913b183/"),
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });



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

            #region Swagger middleware

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            #endregion

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
