using System;
using System.Runtime.Intrinsics.X86;
using Model_Validation.Models;
using Model_Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Model_Validation.Results;

using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

app.MapGet("/", HtmlResult () =>
{
    string html = "<h2> Welcome to our API</h2> Our API is used to learn ASP.Net Core";
    return new HtmlResult(html);
});

app.MapPost("/employee", ([FromBody] Employee employee) =>
{
    if (employee is null || employee.Id <= 0)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            {"id", new [] {"Employee is not provided or invalid"}}
        });
    }
    if (!MiniValidator.TryValidate(employee, out var errors))
    {
        return Results.ValidationProblem(errors);
    }
    EmployeesRepository.AddEmployee(employee);
    return TypedResults.Created($"/employee/{employee.Id}", employee);
});

app.MapGet("/employee", () =>
{
    var employees = EmployeesRepository.GetEmployees();
    return employees;
});

app.MapGet("/register", (Registration? reg) =>
{
    if (!MiniValidator.TryValidate(reg, out var errors))
    {
        return Results.ValidationProblem(errors);
    }
    return Results.Ok($"User with {reg.Email} registered Succesfully");
});

app.MapPost("/register", ([FromBody] Registration? reg) =>
{
    if (!MiniValidator.TryValidate(reg, out var errors))
    {
        return Results.ValidationProblem(errors);
    }
    return Results.Ok($"User with {reg.Email} registered Succesfully");
});

app.Run();
