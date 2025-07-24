using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model_Binding.Models
{
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
}