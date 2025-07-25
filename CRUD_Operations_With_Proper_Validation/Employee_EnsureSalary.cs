using System;
using System.ComponentModel.DataAnnotations;
using CRUD_Operations_With_Proper_Validation.Models;
namespace CRUD_Operations_With_Proper_Validation
{
    public class Employee_EnsureSalary : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var employee = context.ObjectInstance as Employee;

            if (employee is not null && !string.IsNullOrWhiteSpace(employee.Position) && employee.Position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                if (employee.Salary < 100000)
                {
                    return new ValidationResult("A manager Salary has to be greater or Equal to Rs.100000");
                }
            }
            return ValidationResult.Success;
        }

    }
}