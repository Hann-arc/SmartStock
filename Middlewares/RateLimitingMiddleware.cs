using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SmartStockAI.models;
using SmartStockAI.Services;

namespace SmartStockAI.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
        RateLimitingService rateLimiter,
        UserManager<AppUser> userManager)
        {
            var endpoint = context.Request.Path.Value?.ToLower() ?? "";

            // Skip rate limiting for auth endpoints
            if (endpoint.Contains("/auth/") || endpoint.Contains("/swagger"))
            {
                await _next(context);
                return;
            }

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "Staff";

            // Get rate limit policy based on role
            (int maxRequests, int windowMinutes) = userRole switch
            {
                "Admin" => (500, 1),  // 500 requests/minute for Admin
                _ => (100, 1)         // 100 requests/minute for Staff/others
            };

            var cacheKey = $"{userId}:{endpoint}";
            if (rateLimiter.IsRateLimited(cacheKey, maxRequests, windowMinutes))
            {
                context.Response.StatusCode = 429;
                context.Response.Headers["Retry-After"] = "60";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "rate_limit_exceeded",
                    message = "Too many requests. Please try again later.",
                    retryAfter = 60
                });
                return;
            }

            await _next(context);

        }
    }
}