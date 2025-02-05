using System.Text.Json;
using System.Text;
using TagsApi.Common;

namespace TagsApi.Middlewares
{
    public class ExceptionLoggerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionLoggerMiddleware> _logger;
        private readonly IProblemDetailsFactory _problemDetailsFactory;
        public ExceptionLoggerMiddleware(IProblemDetailsFactory problemDetailsFactory, ILogger<ExceptionLoggerMiddleware> logger)
        {
            _problemDetailsFactory = problemDetailsFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";
                
                var problemDetails = _problemDetailsFactory.CreateProblemDetails(ex);
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails), Encoding.UTF8);
            }
        }
    }
}