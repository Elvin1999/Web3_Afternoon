namespace Web3_Afternoon.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Something went wrong",
                    statusCode = 500,
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
