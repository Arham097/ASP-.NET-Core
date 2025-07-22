using System.Text.Json;

var builder = WebApplication.CreateBuilder(args); //WebApplication.CreateBuilder(args); Helps to Create Kestral Server

var app = builder.Build(); // builder.Build() instantiate the web app

// Simple middleware handling endpoints 
app.Run(async (HttpContext context) =>
{
    if (context.Request.Path.StartsWithSegments("/"))
    {
        context.Response.Headers["Content-Type"] = "text/html";
        await context.Response.WriteAsync($"The methods is: {context.Request.Method}</br>");
        await context.Response.WriteAsync($"The path is: {context.Request.Path}</br>");

        await context.Response.WriteAsync("<ul>");
        foreach (var key in context.Request.Headers.Keys)
        {
            await context.Response.WriteAsync($"<li><b>{key}</b>: {context.Request.Headers[key]}</li>");
        }
        await context.Response.WriteAsync("</ul>");
    }
    else if (context.Request.Path.StartsWithSegments("/employees"))
    {
        if (context.Request.Method == "GET")
        {

            context.Response.Headers["Content-Type"] = "text/html";
            if (context.Request.Query.ContainsKey("id"))
            {
                var id = context.Request.Query["id"];
                if (int.TryParse(id, out int empId))
                {
                    var emp = EmployeesRepository.GetEmployeeById(empId);
                    if (emp is not null)
                    {
                        await context.Response.WriteAsync($"{emp.Name}: {emp.Position}</br>");
                    }
                    else
                    {

                        // context.Response.StatusCode = 404;
                        await context.Response.WriteAsync("Employee Not Found");
                    }
                }
            }
            else
            {

                var employees = EmployeesRepository.GetEmployees();

                context.Response.StatusCode = 200;
                foreach (Employee employee in employees)
                {
                    await context.Response.WriteAsync($"{employee.Name}: {employee.Position}</br>");
                }
            }
        }
        else if (context.Request.Method == "POST")
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var employee = JsonSerializer.Deserialize<Employee>(body);
            EmployeesRepository.AddEmployee(employee);
            context.Response.StatusCode = 201;

        }
        else if (context.Request.Method == "PUT")
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


        }
        else if (context.Request.Method == "DELETE")
        {
            if (context.Request.Query.ContainsKey("id"))
            {
                var id = context.Request.Query["id"];
                if (int.TryParse(id, out int employeeId))
                {
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
                }
            }

        }
    }
    else
    {
        context.Response.StatusCode = 404;
    }
});

app.Run();


static class EmployeesRepository
{
    private static List<Employee> employees = new List<Employee>
    {
        new Employee(1, "Arham", "Software Engineer", 40000),
        new Employee(2, "Shayan", "HR", 50000),
        new Employee(3, "Bilal", "QA", 55000),
    };

    public static List<Employee> GetEmployees() => employees;
    public static Employee? GetEmployeeById(int empId)
    {
        return employees.FirstOrDefault(x => x.Id == empId);

    }
    public static void AddEmployee(Employee? employee)
    {
        if (employee is not null)
        {
            employees.Add(employee);
        }
    }
    public static bool UpdateEmployee(Employee? employee)
    {
        var emp = employees.FirstOrDefault((x) => x.Id == employee.Id);
        if (emp is not null)
        {
            emp.Name = employee.Name;
            emp.Position = employee.Position;
            emp.Salary = employee.Salary;

            return true;
        }
        return false;
    }

    public static bool DeleteEmployee(int employeeId)
    {
        var emp = employees.FirstOrDefault(x => x.Id == employeeId);
        if (emp is not null)
        {
            employees.Remove(emp);
            return true;
        }
        return false;
    }
}

public class Employee
{
    public int Id { set; get; }
    public string Name { set; get; }

    public string Position { set; get; }
    public int Salary { set; get; }

    public Employee(int id, string name, string position, int salary)
    {
        Id = id;
        Name = name;
        Position = position;
        Salary = salary;
    }
}