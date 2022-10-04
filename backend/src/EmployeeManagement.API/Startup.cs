using AutoMapper;
using EmployeeManagement.API.Middlewares;
using EmployeeManagement.Core.Extensions.ModelState;
using EmployeeManagement.Core.Mappings;
using EmployeeManagement.DataAccess.Persistance.Contexts;
using EmployeeManagement.Service.Business.Abstracts;
using EmployeeManagement.Service.Business.Implementations;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
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
        public (string API, string Core, string DataAccess, string Service) AssemplyNames { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            AssemplyNames = (API: "EmployeeManagement.API", Core: "EmployeeManagement.Core", DataAccess: "EmployeeManagement.DataAccess", Service: "EmployeeManagement.Service");
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            #region Database context

            services.AddDbContext<EmployeeManagementContext>(option =>
            {
                option.UseNpgsql(
                        Configuration.GetConnectionString("Database"),
                        npgsqlOptions =>
                        {
                            npgsqlOptions.MigrationsAssembly(AssemplyNames.DataAccess);
                            npgsqlOptions.UseNetTopologySuite();
                        }
                    );
            });

            #endregion

            #region Routing configurations

            //Lowercase Routing
            services.AddRouting(options => options.LowercaseUrls = true);

            #endregion

            #region Services


            //Business
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IDepartmentService, DepartmentService>();


            //Mappers
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies(), ServiceLifetime.Singleton);

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
                //Collect all referenced projects output XML document file paths  
                var currentAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                    .Union(new AssemblyName[] { currentAssembly.GetName() })
                    .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                    .Where(f => File.Exists(f)).ToArray();
                
                Array.ForEach(xmlDocs, (d) =>
                {
                    options.IncludeXmlComments(d);
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddFluentValidationRulesToSwagger();

            #endregion

            #region FluentValidation

            var dtoValidationsAssembly = AssemplyNames.Core;

            services.AddFluentValidation(fv => 
                    fv.RegisterValidatorsFromAssembly(Assembly.Load(dtoValidationsAssembly)));

            #endregion

            #region Data protection keys

            services.AddDataProtection()
                .PersistKeysToDbContext<EmployeeManagementContext>();

            #endregion

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                    new BadRequestObjectResult(new { Errors = actionContext.ModelState.SerializeErrors() });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();

            #region Swagger middleware

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
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
