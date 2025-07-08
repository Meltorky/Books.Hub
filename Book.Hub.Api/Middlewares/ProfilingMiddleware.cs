using System.Diagnostics;

namespace Books.Hub.Api.Middlewares
{
    public class ProfilingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProfilingMiddleware> _logger;

        public ProfilingMiddleware(RequestDelegate next, ILogger<ProfilingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                $"Request {context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms to execute");
        }
    }

}
