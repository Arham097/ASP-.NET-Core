
using CRUD_Operations_With_Proper_Validation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MiniValidation;

namespace CRUD_Operations_With_Proper_Validation.Endpoints
{
    public static class Employee_Endpoints
    {
        public static void MapEmployeesEndpoints(this WebApplication app)
        {
            app.MapGet("/employee", () =>
        {
            var employees = EmployeesRepository.GetEmployees();
            return TypedResults.Ok(employees);
        });

            app.MapGet("/employee/{id:int}", (int id) =>
            {
                var employee = EmployeesRepository.GetEmployeeById(id);
                return employee is not null ?
                    TypedResults.Ok(employee) :
                    Results.ValidationProblem(new Dictionary<string, string[]>
                    {
            {"id", new[] {$"Employee with this id: {id} not found"}}
                    }, statusCode: 404);
            });

            app.MapPost("/employee", ([FromBody] Employee employee) =>
            {
                if (!MiniValidator.TryValidate(employee, out var errors))
                {
                    return Results.ValidationProblem(errors, statusCode: 400);
                }
                EmployeesRepository.AddEmployee(employee);
                return TypedResults.Created($"/employee/{employee.Id}", employee);
            });

            app.MapPut("/employee/{id:int}", (int id, Employee employee) =>
            {
                if (!MiniValidator.TryValidate(employee, out var errors))
                {
                    return Results.ValidationProblem(errors, statusCode: 400);
                }

                return EmployeesRepository.UpdateEmployee(employee) ? TypedResults.Ok("Employee Updated Succesfuly")
                : Results.ValidationProblem(new Dictionary<string, string[]>
                {
        {"id", new [] {"Employee with this is not found"}}
                }, statusCode: 404);
            });


            app.MapDelete("/employee/{id:int}", (int id) =>
            {
                return EmployeesRepository.DeleteEmployee(id) ? TypedResults.NoContent()
             : Results.ValidationProblem(new Dictionary<string, string[]>
             {
        {"id", new [] {"Employee with this is not found"}}
             }, statusCode: 404);
            });


        }
    }
}