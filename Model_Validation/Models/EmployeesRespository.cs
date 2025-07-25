using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model_Validation.Models
{
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
                var maxId = employees.Max(x => x.Id);
                employee.Id = maxId + 1;
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

}