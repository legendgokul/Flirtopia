using System.Net;
using System.Text.Json;
using ApiProject.Data.CustomModels.ApiException;

namespace ApiProject.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiException> _logger;
        private readonly IHostEnvironment _env;

        //constructor
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ApiException> logger, IHostEnvironment env)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync( HttpContext httpContext ) 
        {
            try
            {
                await _next(httpContext);

            }catch(Exception ex)
            {
                // logic to perform when exception occured.
                _logger.LogError(ex,ex.Message);     // logging ex and ex.message into terminal.
                
                // need to create an api response to send back to requester.
                httpContext.Response.ContentType = "application/json"; 
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //create response object
                var response = _env.IsDevelopment()
                ? new ApiException((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace?.ToString())
                : new ApiException((int)HttpStatusCode.InternalServerError,ex.Message,"Internal Server Error.");

                var SerializedJson  = JsonSerializer.Serialize(response);

                //write a given string to response body 
                await httpContext.Response.WriteAsync(SerializedJson);

            }
        }



    }   
}