using Ecovadis.API.Infrastructures;
using Ecovadis.DL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ecovadis.API.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // must be awaited
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // if it's not one of the expected exception, set it to 500
            var code = HttpStatusCode.InternalServerError;           
            if (exception is ArgumentNullException) code = HttpStatusCode.BadRequest;
            else if (exception is HttpRequestException) code = HttpStatusCode.BadRequest;
            else if (exception is TableSideException) code = HttpStatusCode.BadRequest;
            else if (exception is EntityNullException) code = HttpStatusCode.NotFound;
            else if (exception is FinishedMatchException) code = HttpStatusCode.Forbidden;
            else if (exception is DbUpdateConcurrencyException) code = HttpStatusCode.Forbidden;
            else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;
            return WriteExceptionAsync(context, exception, code);
        }

        private static Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            return response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = new ErrorResponseModel()
                {
                    Code = (int)code,
                    Message = exception.Message,
                    Exception = exception.GetType().Name
                }
            }));
        }
    }
}
