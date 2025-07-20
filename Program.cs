using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Hello World");
app.Run(async (HttpContext context) =>
{
    if (context.Request.Path.StartsWithSegments("/"))
    {
        await context.Response.WriteAsync($"The methods is: {context.Request.Method}\r\n");
        await context.Response.WriteAsync($"The path is: {context.Request.Path}\r\n");

        foreach (var key in context.Request.Headers.Keys)
        {
            await context.Response.WriteAsync($"{key}: {context.Request.Headers[key]}\r\n");
        }
    }
    else if (context.Request.Path.StartsWithSegments("/employees"))
    {
        if (context.Request.Method == "GET")
        {
            var employees = EmployeesRepository.GetEmployees();

            foreach (Employee employee in employees)
            {
                await context.Response.WriteAsync($"{employee.Name}: {employee.Position}\r\n");
            }
        }
        else if (context.Request.Method == "POST")
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var employee = JsonSerializer.Deserialize<Employee>(body);
            EmployeesRepository.AddEmployee(employee);

        }
        else if (context.Request.Method == "PUT")
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var employee = JsonSerializer.Deserialize<Employee>(body);
            var result = EmployeesRepository.UpdateEmployee(employee);
            if (result)
            {
                await context.Response.WriteAsync("Employee Updated Succesfully");
            }
            else
            {
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
                            await context.Response.WriteAsync("Employee Deleted Succesfully");
                        }
                        else
                        {
                            await context.Response.WriteAsync("Employee Not Found");

                        }
                    }
                    else
                    {
                        await context.Response.WriteAsync("Your are not authorized for this action");
                    }
                }
            }

        }
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