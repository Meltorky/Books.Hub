namespace Books.Hub.Api.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;

        // In-memory store for IP request counts and timestamps
        private static readonly Dictionary<string, (int Count, DateTime StartTime)> _requests = new();
        private static readonly object _lock = new();

        // Settings (can be moved to appsettings or IOptions later)
        private const int MAX_REQUESTS = 5;
        private const int TIME_WINDOW_SECONDS = 10;

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;
            bool isRateLimited = false;

            lock (_lock)
            {
                if (_requests.TryGetValue(ip, out var entry))
                {
                    // If within time window, check the request count
                    if ((now - entry.StartTime).TotalSeconds <= TIME_WINDOW_SECONDS)
                    {
                        if (entry.Count >= MAX_REQUESTS)
                        {
                            isRateLimited = true;
                            _logger.LogWarning("Rate limit exceeded for IP {IP}", ip);
                        }
                        else
                        {
                            _requests[ip] = (entry.Count + 1, entry.StartTime);
                            _logger.LogInformation("Request #{Count} from IP {IP}", entry.Count + 1, ip);
                        }
                    }
                    else
                    {
                        // Reset count and time window
                        _requests[ip] = (1, now);
                        _logger.LogInformation("Resetting rate limit window for IP {IP}", ip);
                    }
                }
                else
                {
                    // First request from this IP
                    _requests[ip] = (1, now);
                    _logger.LogInformation("First request from IP {IP}", ip);
                }
            }

            if (isRateLimited)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                return;
            }

            // Continue to the next middleware
            await _next(context);
        }
    }

}
