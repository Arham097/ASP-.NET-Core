using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPDOTNETCORE.Custom_Middleware
{
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("My Custom Middleware: Before Calling Next\r\n");
            await next(context);
            await context.Response.WriteAsync("My Custom Middleware: After Calling Next\r\n");

        }
    }
}