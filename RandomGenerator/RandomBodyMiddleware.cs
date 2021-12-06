using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RandomGenerator
{
    public class RandomBodyMiddleware<T>
    {
        private readonly RequestDelegate _next;
        private int counter = 0;

        public RandomBodyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IGenerator<T> generator)
        {
            counter++;
            httpContext.Response.ContentType = "text/html;charset=utf-8";
            await httpContext.Response.WriteAsync($"Запрос №{counter}; " +
                $"Сгенерировано значение: {generator.Value}");
        }
    }
}
