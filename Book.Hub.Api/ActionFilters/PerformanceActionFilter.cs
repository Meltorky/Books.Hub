using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Books.Hub.Api.ActionFilters
{
    public class PerformanceActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Force GC to get a stable memory baseline
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryBefore = GC.GetTotalMemory(true);
            var stopwatch = Stopwatch.StartNew();

            var resultContext = await next(); // Execute the action

            stopwatch.Stop();
            long memoryAfter = GC.GetTotalMemory(false);

            long memoryUsed = memoryAfter - memoryBefore;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"[PERF] {context.ActionDescriptor.DisplayName} → " +
                $"{stopwatch.ElapsedMilliseconds} ms | " +
                $"Memory: {memoryUsed /1024 /1024} MB");
            Console.ResetColor();
        }
    }
}
