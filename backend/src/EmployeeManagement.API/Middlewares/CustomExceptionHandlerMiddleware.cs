using EmployeeManagement.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<CustomExceptionHandlerMiddleware>();
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApplicationException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await LogFailedRequestAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Failures);
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedException _:
                    code = HttpStatusCode.Unauthorized;
                    break;
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)code;

            if (result == string.Empty) result = JsonConvert.SerializeObject(new { error = exception.Message });

            return context.Response.WriteAsync(result);
        }

        private async Task LogFailedRequestAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(
                "Failed Request\n" +
                "\tSchema: {Schema}\n" +
                "\tHost: {Host}\n" +
                "\tMethod: {Method}\n" +
                "\tPath: {Path}\n" +
                "\tQueryString: {QueryString}\n" +
                "\tErrorMessage: {ErrorMessage}\n" +
                "\tStacktrace (5):\n{StackTrace}",
                context.Request?.Scheme,
                context.Request?.Host,
                context.Request?.Method,
                context.Request?.Path,
                context.Request?.QueryString,
                exception.Message,
                exception.StackTrace?.Split('\n').Take(5).Aggregate((a, b) => a + "\n" + b)
            );

            var code = HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(_env.IsDevelopment() ? exception : "Internal server error happened.");

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
