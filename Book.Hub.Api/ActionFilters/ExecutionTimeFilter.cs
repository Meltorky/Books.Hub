using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Books.Hub.Api.ActionFilters
{
    public class ExecutionTimeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            var resultContext = await next(); // Execute the action method
            stopwatch.Stop();

            var executionTime = stopwatch.ElapsedMilliseconds;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} executed in {executionTime} ms");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

}
