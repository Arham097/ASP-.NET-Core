// Model Binding
// Definition: It Extracts the data from http request to .Net objects as parameter in endpoint handler.
// Model binding can be done through (Route Values, Query String, Headers, Body)

// Binding Source Priorities
// 1. Explicit
// 2. BindAsync
// 3. Binding primitive types to route parameters
// 4. Binding primitive type to query string
// 5. Binding Array to query String
// 6. Body


using Microsoft.AspNetCore.Mvc;
using Model_Binding.Models;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // 1. Model Binding Through Route Values
    // {You can explicitly defind [FromRoute] to bind with Route Values otherwise default it takes routes as it has first priority}
    // Name of Route Parameter and binding variable should be same
    // But if you want to change it so can done through in a way  ("/employee/{id:int}", ([FromRoute (Name="id)] int ids) so now the id bind with ids also
    // endpoints.MapGet("/employee/{id:int}", (int id) =>
    // {
    //     var employee = EmployeesRepository.GetEmployeeById(id);
    //     return employee;
    // });

    // 2. Model Binding Through Query String
    // if you dont provide Route Parameter and pass values as query string so it accepts it from there
    // Can be Explicitly defined through [FromQuery]
    // endpoints.MapGet("/employee", (int id) =>
    // {
    //     var employee = EmployeesRepository.GetEmployeeById(id);
    //     return employee;
    // });

    // 3.  Model Binding Through Header
    // You have to explicitly insert [FromHeader]
    // endpoints.MapGet("/employee", ([FromHeader] int id) =>
    // {
    //     var employee = EmployeesRepository.GetEmployeeById(id);
    //     return employee;
    // });

    //4. Can use GroupParameters using (AsParameters);
    // endpoints.MapGet("/employee/{id:int}", ([AsParameters] GetEmployeeParameters param) =>
    // {
    //     var employee = EmployeesRepository.GetEmployeeById(param.Id);
    //     employee.Name = param.Name;
    //     employee.Position = param.Position;
    //     return employee;
    // });

    // 5. Bind Arrays to query string or headers
    endpoints.MapGet("/employee", ([FromQuery(Name = "id")] int[] ids) =>
    {
        var employess = EmployeesRepository.GetEmployees();
        var emps = employess.Where(x => ids.Contains(x.Id)).ToList();
        return emps;
    });

    // 6. Bind with HttpBody
    endpoints.MapPost("/employee", (Employee employee) =>
    {
        if (employee is null || employee.Id <= 0)
        {
            return "Employee is not provided or invalid";
        }
        EmployeesRepository.AddEmployee(employee);
        return "Employee Added Succesfully";

    });

    // 7. Custom Binding With (BuildAsync) Method

    endpoints.MapGet("/people", (Person? p) =>
    {
        return $"Id is {p.Id} , and Name is {p.Name}";
    });
});

app.Run();

public class GetEmployeeParameters
{
    [FromRoute]
    public int Id { set; get; }

    [FromQuery]
    public string Name { set; get; }

    [FromHeader]
    public string Position { set; get; }
}


class Person
{
    public int Id { set; get; }
    public string? Name { set; get; }

    public static ValueTask<Person?> BindAsync(HttpContext context)
    {
        var idStr = context.Request.Query["id"];
        var nameStr = context.Request.Query["name"];
        if (int.TryParse(idStr, out var id))
        {
            return new ValueTask<Person?>(new Person { Id = id, Name = nameStr });
        }
        return new ValueTask<Person?>(Task.FromResult<Person?>(null));
    }
}
