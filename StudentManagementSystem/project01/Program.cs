using project01.Data;
using project01.Models;
using Microsoft.EntityFrameworkCore;

namespace project01
{
    class Program
    {
        static void Main(string[] args)
        {
            using AppDbContext db = new AppDbContext();

            SeedData(db);

            while (true)
            {
                Console.WriteLine("\n===== Student Course Management System =====");
                Console.WriteLine("1- Add Student");
                Console.WriteLine("2- Update Student");
                Console.WriteLine("3- Delete Student");
                Console.WriteLine("4- Get All Students");
                Console.WriteLine("5- Search Student");
                Console.WriteLine("6- LINQ Reports");
                Console.WriteLine("7- Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent(db);
                        break;
                    case "2":
                        UpdateStudent(db);
                        break;
                    case "3":
                        DeleteStudent(db);
                        break;
                    case "4":
                        GetAllStudents(db);
                        break;
                    case "5":
                        SearchStudent(db);
                        break;
                    case "6":
                        LinqReports(db);
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void SeedData(AppDbContext db)
        {
            if (db.Departments.Any())
                return;

            Department d1 = new Department { Name = "IT" };
            Department d2 = new Department { Name = "Business" };
            Department d3 = new Department { Name = "Engineering" };

            Course c1 = new Course { Title = "C#", Hours = 3 };
            Course c2 = new Course { Title = "Database", Hours = 4 };
            Course c3 = new Course { Title = "Math", Hours = 3 };
            Course c4 = new Course { Title = "Management", Hours = 2 };
            Course c5 = new Course { Title = "Networking", Hours = 3 };

            List<Student> students = new List<Student>
            {
                new Student { Name = "Ali", Age = 19, Email = "ali@gmail.com", Department = d1, Courses = new List<Course>{ c1, c2 } },
                new Student { Name = "Sara", Age = 22, Email = "sara@gmail.com", Department = d1, Courses = new List<Course>{ c1, c5 } },
                new Student { Name = "Mona", Age = 21, Email = "mona@gmail.com", Department = d2, Courses = new List<Course>{ c3, c4 } },
                new Student { Name = "Ahmed", Age = 25, Email = "ahmed@gmail.com", Department = d3, Courses = new List<Course>{ c2, c3 } },
                new Student { Name = "Noor", Age = 20, Email = "noor@gmail.com", Department = d2, Courses = new List<Course>{ c1, c4 } },
                new Student { Name = "Khalid", Age = 27, Email = "khalid@gmail.com", Department = d3, Courses = new List<Course>{ c5, c2 } },
                new Student { Name = "Huda", Age = 23, Email = "huda@gmail.com", Department = d1, Courses = new List<Course>{ c1, c3 } },
                new Student { Name = "Omar", Age = 18, Email = "omar@gmail.com", Department = d2, Courses = new List<Course>{ c4, c5 } },
                new Student { Name = "Laila", Age = 24, Email = "laila@gmail.com", Department = d3, Courses = new List<Course>{ c2, c5 } },
                new Student { Name = "Yousuf", Age = 26, Email = "yousuf@gmail.com", Department = d1, Courses = new List<Course>{ c1, c2, c5 } }
            };

            db.Students.AddRange(students);
            db.SaveChanges();

            Console.WriteLine("Seed data inserted successfully.");
        }

        static void AddStudent(AppDbContext db)
        {
            try
            {
                Console.Write("Enter name: ");
                string name = Console.ReadLine();

                Console.Write("Enter age: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Enter email: ");
                string email = Console.ReadLine();

                bool emailExists = db.Students.Any(s => s.Email == email);
                if (emailExists)
                {
                    Console.WriteLine("This email already exists.");
                    return;
                }

                Console.WriteLine("Departments:");
                foreach (var d in db.Departments.ToList())
                {
                    Console.WriteLine($"{d.Id}- {d.Name}");
                }

                Console.Write("Enter Department Id: ");
                int departmentId = int.Parse(Console.ReadLine());

                Student student = new Student
                {
                    Name = name,
                    Age = age,
                    Email = email,
                    DepartmentId = departmentId
                };

                db.Students.Add(student);
                db.SaveChanges();

                Console.WriteLine("Student added successfully.");
            }
            catch
            {
                Console.WriteLine("Error while adding student.");
            }
        }

        static void UpdateStudent(AppDbContext db)
        {
            Console.Write("Enter Student Id: ");
            int id = int.Parse(Console.ReadLine());

            Student student = db.Students.Find(id);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.Write("Enter new name: ");
            student.Name = Console.ReadLine();

            Console.Write("Enter new age: ");
            student.Age = int.Parse(Console.ReadLine());

            Console.Write("Enter new email: ");
            student.Email = Console.ReadLine();

            db.SaveChanges();

            Console.WriteLine("Student updated successfully.");
        }

        static void DeleteStudent(AppDbContext db)
        {
            Console.Write("Enter Student Id: ");
            int id = int.Parse(Console.ReadLine());

            Student student = db.Students.Find(id);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            db.Students.Remove(student);
            db.SaveChanges();

            Console.WriteLine("Student deleted successfully.");
        }

        static void GetAllStudents(AppDbContext db)
        {
            var students = db.Students
                .Include(s => s.Department)
                .Include(s => s.Courses)
                .ToList();

            foreach (var s in students)
            {
                Console.WriteLine($"\nName: {s.Name}");
                Console.WriteLine($"Age: {s.Age}");
                Console.WriteLine($"Department: {s.Department.Name}");
                Console.WriteLine("Courses: " + string.Join(", ", s.Courses.Select(c => c.Title)));
            }
        }

        static void SearchStudent(AppDbContext db)
        {
            Console.Write("Enter name or age: ");
            string input = Console.ReadLine();

            var result = db.Students
                .Include(s => s.Department)
                .Where(s => s.Name.Contains(input) || s.Age.ToString() == input)
                .ToList();

            foreach (var s in result)
            {
                Console.WriteLine($"{s.Name} - {s.Age} - {s.Department.Name}");
            }
        }

        static void LinqReports(AppDbContext db)
        {
            Console.WriteLine("\nStudents older than 20:");
            var olderThan20 = db.Students.Where(s => s.Age > 20).ToList();
            foreach (var s in olderThan20)
                Console.WriteLine(s.Name);

            Console.WriteLine("\nStudent names only:");
            var names = db.Students.Select(s => s.Name).ToList();
            foreach (var name in names)
                Console.WriteLine(name);

            Console.WriteLine("\nStudents ordered by age:");
            var ordered = db.Students.OrderBy(s => s.Age).ToList();
            foreach (var s in ordered)
                Console.WriteLine($"{s.Name} - {s.Age}");

            Console.WriteLine($"\nTotal Students: {db.Students.Count()}");

            Console.WriteLine($"Any student age > 25: {db.Students.Any(s => s.Age > 25)}");

            Console.WriteLine($"Average Age: {db.Students.Average(s => s.Age)}");

            Console.WriteLine("\nGroup Students By Department:");
            var groups = db.Students
                .Include(s => s.Department)
                .GroupBy(s => s.Department.Name)
                .ToList();

            foreach (var group in groups)
            {
                Console.WriteLine($"\nDepartment: {group.Key}");
                foreach (var student in group)
                {
                    Console.WriteLine(student.Name);
                }
            }

            Console.WriteLine("\nAdvanced Department Report:");
            var report = db.Students
                .Include(s => s.Department)
                .GroupBy(s => s.Department.Name)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    NumberOfStudents = g.Count(),
                    AverageAge = g.Average(s => s.Age)
                })
                .ToList();

            foreach (var item in report)
            {
                Console.WriteLine($"{item.DepartmentName} | Students: {item.NumberOfStudents} | Avg Age: {item.AverageAge}");
            }
        }
    }
}

