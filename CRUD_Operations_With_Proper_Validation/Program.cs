using CRUD_Operations_With_Proper_Validation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MiniValidation;
using CRUD_Operations_With_Proper_Validation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();
app.MapEmployeesEndpoints();



app.Run();
