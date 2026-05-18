using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class Student
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age { get; set; }
        public string Email { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
