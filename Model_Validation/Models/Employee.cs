using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Model_Validation;

namespace Model_Validation.Models
{
    public class Employee
    {
        public int Id { set; get; }

        [Required]
        public string Name { set; get; }

        public string Position { set; get; }
        [Required]
        [Range(50000, 200000)]
        [Employee_EnsureSalary]
        public int Salary { set; get; }

        public Employee(int id, string name, string position, int salary)
        {
            Id = id;
            Name = name;
            Position = position;
            Salary = salary;
        }
    }
}