using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomGenerator
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RandomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public RandomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IGenerator<int> generator)
        {
            httpContext.Response.Headers.Add("Random-Value", generator.Value.ToString());
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RandomHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseRandomHeaderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RandomHeaderMiddleware>();
        }
    }
}
