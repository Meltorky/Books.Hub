using FluentAssertions;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Api.Middlewares;

namespace UnitTests.API.MiddleWars
{
    public class ExceptionHandlingMiddlewareTests
    {
        
        // Helper method to create a fake HttpContext with a writable response stream
        private HttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream(); // So we can read what was written to the response
            return context;
        }

        // Helper method to read the response body content as string
        private string GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin); // Move to start of stream
            using var reader = new StreamReader(response.Body);
            return reader.ReadToEnd();
        }



        [Fact]
        public async Task InvokeAsync_WhenArgumentExceptionThrown_Returns400()
        {
            // Arrange
            var context = CreateHttpContext(); // Set up HttpContext
            var fakeLogger = A.Fake<ILogger<ExceptionHandlingMiddleware>>(); // Fake logger
            var middleware = new ExceptionHandlingMiddleware(
                _ => throw new ArgumentException("Invalid argument"), // Simulate service throwing ArgumentException
                fakeLogger
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest); // Verify status code
            var body = GetResponseBody(context.Response); // Read response content
            body.Should().Contain("Invalid argument"); // Verify error message
        }



        [Fact]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_Returns404()
        {
            // Arrange
            var context = CreateHttpContext();
            var fakeLogger = A.Fake<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(
                _ => throw new NotFoundException("Author not found"),
                fakeLogger
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            var body = GetResponseBody(context.Response);
            body.Should().Contain("Author not found");
        }



        [Fact]
        public async Task InvokeAsync_WhenOperationCanceledExceptionThrown_Returns499()
        {
            // Arrange
            var context = CreateHttpContext();
            var fakeLogger = A.Fake<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(
                _ => throw new OperationCanceledException(),
                fakeLogger
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(499);
            var body = GetResponseBody(context.Response);
            body.Should().Contain("Request was canceled");
        }



        [Fact]
        public async Task InvokeAsync_WhenUnhandledExceptionThrown_Returns500()
        {
            // Arrange
            var context = CreateHttpContext();
            var fakeLogger = A.Fake<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(
                _ => throw new Exception("Something went wrong"), // Unexpected error
                fakeLogger
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            var body = GetResponseBody(context.Response);
            body.Should().Contain("An unexpected error occurred");
        }



        [Fact]
        public async Task InvokeAsync_WhenNoExceptionThrown_CallsNextMiddleware()
        {
            // Arrange
            var context = CreateHttpContext();
            var wasCalled = false;

            // Simulate a next middleware that simply sets a flag
            RequestDelegate next = ctx =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            var fakeLogger = A.Fake<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(next, fakeLogger);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            wasCalled.Should().BeTrue(); // Ensure next delegate was called
            context.Response.StatusCode.Should().Be(200);
        }
    }
}