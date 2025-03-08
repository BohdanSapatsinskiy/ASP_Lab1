using static MiddlewareSandbox.Program;

namespace MiddlewareSandbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRequestCount();

            app.UseLoggingMiddleware();

            app.UseCustomQueryMiddleware();

            app.UseApiKeyMiddleware();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        // Middleware for counting requests
        public class RequestCountMiddleware
        {
            private readonly RequestDelegate _next;
            private static int _requestCount = 0;

            public RequestCountMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                _requestCount++;
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("X-Request-Count", _requestCount.ToString());
                    return Task.CompletedTask;
                });

                await _next(context);
            }
        }
    }

    // Middleware для обробки запиту
    public static class RequestCountMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCount(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCountMiddleware>();
        }
    }
    // Middleware для кастомного параметра запиту
    public class CustomQueryMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomQueryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var queryParams = context.Request.Query;

            if (queryParams.ContainsKey("custom"))
            {
                await context.Response.WriteAsync("You’ve hit a custom middleware!");
            }
            else
            {
                await _next(context);
            }
        }
    }

    public static class CustomQueryMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomQueryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomQueryMiddleware>();
        }
    }

    //Middleware для логування методів запиту
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Request Method: {context.Request.Method}, Path: {context.Request.Path}");
            await _next(context);
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }

    // Middleware для перевірки API-ключа
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _validApiKey = "11111111111111111111111111111111";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey) || apiKey != _validApiKey)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            await _next(context);
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
