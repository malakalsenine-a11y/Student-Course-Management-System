using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Hours { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
