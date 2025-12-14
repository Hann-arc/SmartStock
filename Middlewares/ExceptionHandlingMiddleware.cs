using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using SmartStockAI.Exceptions;

namespace SmartStockAI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden);
            }
           catch (DuplicateEntityException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict);
            }
            catch (BusinessRuleException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = (int)statusCode,
                error = exception.GetType().Name,
                message = exception.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}