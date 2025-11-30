using Books.Hub.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Books.Hub.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
            catch (ArgumentException ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status400BadRequest, "Invalid argument", LogLevel.Warning);
            }
            catch (ValidationException ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status400BadRequest, "Validation failed", LogLevel.Warning);
            }
            catch (NotFoundException ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status404NotFound, "Resource not found", LogLevel.Warning);
            }
            catch (OperationFailedException ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status400BadRequest, "Operation failed", LogLevel.Warning);
            }
            catch (OperationCanceledException ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status499ClientClosedRequest, "Request canceled", LogLevel.Information);
            }
            catch (DuplicateEmailException ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status400BadRequest, "Duplicate Email", LogLevel.Information);
            }
            catch (ForbiddenAccessException ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status400BadRequest, "Forbidden Access", LogLevel.Error);
            }
            catch (Exception ex)
            {
                await WriteProblemAsync(context, ex, StatusCodes.Status500InternalServerError, "Internal server error", LogLevel.Error);
            }
        }

        private async Task WriteProblemAsync(
            HttpContext context,
            Exception ex,
            int statusCode,
            string title,
            LogLevel logLevel)
        {
            var problem = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            // Logging
            _logger.Log(logLevel, ex, "{StatusCode}: {Title}. Path: {Path}", statusCode, title, context.Request.Path);

            // Response
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
