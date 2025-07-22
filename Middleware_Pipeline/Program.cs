// Middleware Pipelines works as a function calls.

/* When Request came Kestral Server calls first middleware in the pipeline , then 1st called 2nd with context object, and 2nd called 3rd
 and so on, when last middleware executed it return resposnse with context obj , the calls now return back to its previous middleware 
 which return back to 1st middleware that return modified context object to Kestral Server*/

/* In some cases , a middleware does not forward calls to next middleware but it start returning call from itself , 
so this process is called (short circuiting) and the middleware who does that called (terminal middleware)*/

using ASPDOTNETCORE.Custom_Middleware;
var builder = WebApplication.CreateBuilder(args);

// Resgister Custome Middleware in Services
builder.Services.AddTransient<MyCustomMiddleware>();
builder.Services.AddTransient<MyCustomExceptionHandler>();
var app = builder.Build();

// Call Custom Exception Middleware to check it is working or not
app.UseMiddleware<MyCustomExceptionHandler>();

// You can create middlewares by using app.Use()
// Middleware #1
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Middleware # 1: Before Calling Next\r\n");
    await next(context);
    await context.Response.WriteAsync("Middleware # 1: After Calling Next\r\n");

});

// Call Custom Middleware
app.UseMiddleware<MyCustomMiddleware>();

// Middleware #2
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // Checking By throwing Exception (You can comment it to see normal result)
    throw new ApplicationException("Exception for testing");
    /////////////////
    await context.Response.WriteAsync("Middleware # 2: Before Calling Next\r\n");
    await next(context); // If we comment any next delegate so then this middleware acts as terminal middleware and response start to return from here without going further
    await context.Response.WriteAsync("Middleware # 2: After Calling Next\r\n");

});

// Or we can use ( app.Run () ) to direct create a terminal middleware

// app.Run(async (context) =>
// {
//     await context.Response.WriteAsync("Middleware # 2: Processed\r\n");
// });


/* Thorugh ( app.Map () ) you can create a branch in pipeline but it does not join to main pipeline (Means 1->2->4->5->5->4->3->2->1) 
so it does not join with main branch containing middleware 3*/

// app.Map("/employee", (appBuilder) =>
// {
//     appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
//     {
//         await context.Response.WriteAsync("Middleware # 4: Before Calling Next\r\n");
//         await next(context);
//         await context.Response.WriteAsync("Middleware # 4: After Calling Next\r\n");
//     });

//     appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
//    {
//        await context.Response.WriteAsync("Middleware # 5: Before Calling Next\r\n");
//        await next(context);
//        await context.Response.WriteAsync("Middleware # 5: After Calling Next\r\n");
//    });
// });

// To create a rejoinable branch which rejoin to main pipeline after executing branch middlewares so you can use ( app.UseWhen() )

app.UseWhen((context) =>
{
    return context.Request.Path.StartsWithSegments("/employee") && context.Request.Query.ContainsKey("id");
},
(appBuilder) =>
{
    appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync("Middleware # 4: Before Calling Next\r\n");
        await next(context);
        await context.Response.WriteAsync("Middleware # 4: After Calling Next\r\n");
    });

    appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync("Middleware # 5: Before Calling Next\r\n");
        await next(context);
        await context.Response.WriteAsync("Middleware # 5: After Calling Next\r\n");
    });
}
);




// Middleware #3
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Middleware # 3: Before Calling Next\r\n");
    await next(context);
    await context.Response.WriteAsync("Middleware # 3: After Calling Next\r\n");

});


app.Run();
