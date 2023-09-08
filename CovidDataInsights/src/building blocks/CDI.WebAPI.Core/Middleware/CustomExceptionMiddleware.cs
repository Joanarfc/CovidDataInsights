using CDI.WebAPI.Core.Utils;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CDI.WebAPI.Core.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next,
                                        ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            // Default to Internal Server Error status code
            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = error.Message;

            if (error is AppException)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = error?.Message;
            }
            else if (error is DbUpdateConcurrencyException)
            {
                statusCode = HttpStatusCode.NotFound;
                errorMessage = "Database concurrency error occurred.";
            }
            else if (error is FileNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                errorMessage = "File not found.";
            }

            _logger.LogError(error, errorMessage);

            response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                statusCode = response.StatusCode,
                message = errorMessage
            });

            await response.WriteAsync(result);
        }
    }
}