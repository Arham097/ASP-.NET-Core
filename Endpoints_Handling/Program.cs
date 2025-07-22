using Endpoints_Handling.Models;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("pos", typeof(PostionConstraint));
});
var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();


app.UseEndpoints(static (endpoints) =>
{
    endpoints.MapGet("/employee", async (HttpContext context) =>
    {
        var employees = EmployeesRepository.GetEmployees();

        context.Response.StatusCode = 200;
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("<h2>Employee</h2>");
        await context.Response.WriteAsync("<ul>");
        foreach (Employee employee in employees)
        {
            await context.Response.WriteAsync($"<li>{employee.Name}: {employee.Position}</li></br>");
        }
        await context.Response.WriteAsync("</ul>");
    });

    endpoints.MapGet("/employee/{id:int}", async (HttpContext context) =>
    {
        var id = context.Request.RouteValues["id"];
        var employeeId = int.Parse(id.ToString());
        var emp = EmployeesRepository.GetEmployeeById(employeeId);
        if (emp is not null)
        {
            await context.Response.WriteAsync($"{emp.Name}: {emp.Position}</br>");
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Employee Not Found");
        }
    });

    endpoints.MapPost("/employee", async (HttpContext context) =>
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var employee = JsonSerializer.Deserialize<Employee>(body);
        EmployeesRepository.AddEmployee(employee);
        context.Response.StatusCode = 201;
    });
    endpoints.MapPut("/employee", async (HttpContext context) =>
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var employee = JsonSerializer.Deserialize<Employee>(body);
        var result = EmployeesRepository.UpdateEmployee(employee);
        if (result)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Employee Updated Succesfully");
            return;
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Employee Not Found");
        }
    });

    // Required route Parameters (like {id})
    endpoints.MapDelete("/employee/{id:int}", async (HttpContext context) =>
    {
        var id = context.Request.RouteValues["id"];
        var employeeId = int.Parse(id.ToString());
        if (context.Request.Headers["Authorization"] == "Arham")
        {
            var result = EmployeesRepository.DeleteEmployee(employeeId);
            if (result)
            {
                context.Response.StatusCode = 204;
                await context.Response.WriteAsync("Employee Deleted Succesfully");
                return;
            }
            else
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Employee Not Found");

            }
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Your are not authorized for this action");
        }

    });

    // Required route Parameters with default values (like {size=medium})

    endpoints.MapGet("/categories/{size=medium}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get Cateogries in  {context.Request.RouteValues["size"]} ");

    });

    // Optional Route Parameters with optoinal values (like {id?})

    endpoints.MapGet("/categories/{size=medium}/{id?}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get Cateogries in  {context.Request.RouteValues["size"]} with id: {context.Request.RouteValues["id"]}");

    });

    // Optional Route Parameters with optoinal values (like {id?})

    endpoints.MapGet("/employee/position/{position:pos}", async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Get Employees under position {context.Request.RouteValues["position"]}");

    });

});

app.Run();

class PostionConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey(routeKey)) return false;
        if (values[routeKey] is null) return false;

        if (values[routeKey].ToString().Equals("manager", StringComparison.OrdinalIgnoreCase) ||
           values[routeKey].ToString().Equals("developer", StringComparison.OrdinalIgnoreCase)) return true;

        return false;
    }
}
// FOr 