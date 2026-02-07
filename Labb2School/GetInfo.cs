using Labb2School.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2School
{
    internal class GetInfo
    {

        internal static void OrderByLastName()
        {
            //Context connecting to the database
            using (var context = new Labb2SchoolContext())
            {
                //ask user in menu if asc or desc 
                bool IsAscending = Menu.StudentSortingMenu();

                if (IsAscending)
                {
                    //get student sorting by last name asc 
                    var studentsOrderByLastName = context.Students
                        .OrderBy(st => st.LastName)
                        .ToList();
                    //send the list to a method 
                    PrintStudentInfo(studentsOrderByLastName);
                }
                else
                {
                    //else get with last name but sort DESC 
                    var studentsOrderByLastName = context.Students
                    .OrderByDescending(st => st.LastName)
                    .ToList();
                    PrintStudentInfo(studentsOrderByLastName);
                }
                Menu.ReadKeyAndClear();
            }
        }
        internal static void OrderByFirstName()
        {
            using (var context = new Labb2SchoolContext())
            {
                bool IsAscending = Menu.StudentSortingMenu();

                if (IsAscending)
                {
                    var studentsOrderByFirstName = context.Students
                    .OrderBy(st => st.FirstName)
                    .ToList();

                    PrintStudentInfo(studentsOrderByFirstName);
                }
                else
                {
                    var studentsOrderByFirstName = context.Students
                   .OrderByDescending(st => st.FirstName)
                   .ToList();

                    PrintStudentInfo(studentsOrderByFirstName);
                }
                Menu.ReadKeyAndClear();
            }
        }
        internal static void PrintStudentInfo(List<Student> students)
        {
            //Recive a list and print 

            Console.WriteLine("Lista med alla studenter:\n");
            int width = 25;
            Console.Write("Förnamn".PadRight(15));
            Console.Write("Efternamn".PadRight(15));
            Console.WriteLine();
            Console.WriteLine(new string('-', width));

            foreach (var student in students)
            {
                Console.Write($"{student.FirstName}".PadRight(15));
                Console.Write($"{student.LastName}".PadRight(15));
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();

        }
        internal static void PrintAllStudentsInfo()
        {
            using (var context = new Labb2SchoolContext())
            {
                var students = context.Students
                    .Include(st => st.Class)
                    .ToList();
               
                Console.WriteLine("Lista med alla studenter:\n");

                int width = 20;
                //Header 
                Console.Write("Id".PadRight(8));
                Console.Write("Namn".PadRight(width));
                Console.Write("Klass".PadRight(8));
                Console.Write("Personnummer".PadRight(width));
                Console.WriteLine();
                Console.WriteLine(new string('-', width * 3));
                
                //Recive a list and print 

                foreach (var student in students)
                {
                    Console.Write(student.StudentId.ToString().PadRight(8));
                    Console.Write($"{student.FirstName} {student.LastName}".PadRight(20));
                    Console.Write(student.Class.ClassName.PadRight(8));
                    Console.Write(student.PersonalNumber.PadRight(20));
                    Console.WriteLine();
                }
                Menu.ReadKeyAndClear();
            }
        }
        internal static void PrintStudentList()
        {
            using var context = new Labb2SchoolContext();

            int count = context.Students.Count();
            Console.WriteLine($"Välj student id mellan 1-{count}");
        }
        internal static void ShowActiveSubject()
        {
            using (var context = new Labb2SchoolContext())
            {
                var subjects = context.Subjects
                    .OrderBy(sub => sub.SubjectName)
                    .ToList();

                Console.WriteLine("Aktiva ämne");

                foreach (var subject in subjects)
                {
                    Console.WriteLine($"{subject.SubjectId}. {subject.SubjectName} {subject.CourseCode}");
                }

                Menu.ReadKeyAndClear();
            }
        }

        internal static void GetStudentsByClass(int classId)
        {
            using (var context = new Labb2SchoolContext())
            {
                var studentsToFind = context.Students
                        .Include(st => st.Class)
                        .Where(st => st.ClassId == classId)
                        .ToList();

                foreach (var student in studentsToFind)
                {
                    Console.WriteLine($"{student.Class.ClassName}. {student.StudentId}. {student.FirstName} {student.LastName} - {student.PersonalNumber}");
                }
                Console.WriteLine("Tryck på en knapp");
                Console.ReadKey();
                Console.Clear();

            }
        }
        //To show classId and className to facilitate user's choice
        internal static void GetClasses()
        {
            using (var context = new Labb2SchoolContext())
            {
                var printClasses = context.Classes.ToList();

                foreach (var c in printClasses)
                {
                    Console.WriteLine($"{c.ClassId}.  {c.ClassName}");
                }
            }
        }
        internal static void AddStaff()
        {
            using (var context = new Labb2SchoolContext())
            {
                Console.WriteLine("Lägga till en ny personal");
                Console.WriteLine("----------------------------");

                Console.WriteLine("Förnamn: ");
                string firstName = Console.ReadLine() ?? "";

                Console.WriteLine("Efternamn: ");
                string lastName = Console.ReadLine() ?? "";

                int roleId = 0;
                bool validInputId = false;
                while (!validInputId)
                {
                    Console.WriteLine("Position: ");
                    GetStaffId();

                    if (!int.TryParse(Console.ReadLine(), out roleId) || roleId < 1 || roleId > 4)
                    {
                        PrintErrorMessage();
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        validInputId = true;
                    }
                }
                //Create a new staff and save in database
                var newStaff = new Staff
                {
                    FirstName = firstName,
                    LastName = lastName,
                    RoleId = roleId

                };
                context.Staff.Add(newStaff);
                context.SaveChanges();

                Console.WriteLine("Ny personal har sparat i systemet.");
                Console.WriteLine("Tryck på en knapp");
                Console.ReadKey();
                Console.Clear();
            }
        }

        internal static void GetStaffId()
        {
            using (var context = new Labb2SchoolContext())
            {
                var roles = context.Roles.ToList();

                foreach (var r in roles)
                {
                    Console.WriteLine($"{r.RoleId}. {r.RoleName}");

                }

            }
        }
        internal static void PrintStaffInfo()
        {
            using (var context = new Labb2SchoolContext())
            {
                //JOIN conecting Staff.RoleId(FK) with Roles.RoleId(PK) and get RoleName 
                var staffWithRoleName = context.Staff
                                .Join(
                                        context.Roles,
                                        s => s.RoleId,
                                        r => r.RoleId,
                                        (s, r) => new
                                        {
                                            s.StaffId,
                                            Name = s.FirstName + " " + s.LastName,
                                            r.RoleName
                                        })
                                .ToList();

                foreach (var s in staffWithRoleName)
                {
                    Console.WriteLine($"{s.StaffId}. {s.Name},   Roll: {s.RoleName}");
                }
                Console.WriteLine("Tryck på en knapp");
                Console.ReadKey();
                Console.Clear();
            }
        }

        internal static void PrintErrorMessage()
        {
            Console.WriteLine("Fel värde, försök igen!");
        }

        //Calculate teachers in each department
        internal static void CountTeacherInDepartment()
        {
            using (var context = new Labb2SchoolContext())
            {
                var TeacherInDepartment = context.Staff
                    .Where(s => s.Role.RoleName == "Teacher")
                    .GroupBy(s => s.Department.DepartmentName)
                    .Select(group => new
                    {
                        Department = group.Key,
                        Count = group.Count(),

                    })
                    .OrderBy(r => r.Department)
                    .ToList();


                foreach (var r in TeacherInDepartment)
                {
                    Console.WriteLine($"{r.Department} har {r.Count} lärare");
                }
                Console.WriteLine("Tryck på en knapp");
                Console.ReadKey();
                Console.Clear();
            }
        }
        //Show students with and without grade before set grade transaction
        internal static void GetStudentsWithAndWithoutGrade()
        {
            using var context = new Labb2SchoolContext();
            var studentsWithAndWithoutGrade = context.Students
                .GroupJoin(
                    context.Grades,
                    st => st.StudentId,
                    g => g.StudentId,
                    (st, grades) => new
                    {
                        StudentId = st.StudentId,
                        StudentName = st.FirstName + " " + st.LastName,
                        HasGrade = context.Grades.Any(g => g.StudentId == st.StudentId && g.Grade1 != null)
                    })
                .ToList();

            foreach (var st in studentsWithAndWithoutGrade)
            {
                Console.WriteLine($"{st.StudentId}. {st.StudentName} - {(st.HasGrade ? "Har betyg" : "Saknar betyg")}");
            }

        }
        //Print list of teachers for user's to set grade 
        internal static void PrintTeachers()
        {
            using (var context = new Labb2SchoolContext())
            {
                var teachers = context.Staff
                    .Where(s => s.Role.RoleName == "Teacher")
                    .Select(s => new
                    {
                        s.StaffId,
                        s.FirstName,
                        s.LastName
                    })
                    .ToList();

                foreach (var teacher in teachers)
                {
                    Console.WriteLine($"{teacher.StaffId}. {teacher.FirstName} {teacher.LastName} ");
                }
            }

        }
        //Get the numbers of all students with id
        internal static int GetMaxStudentId()
        {
            using var context = new Labb2SchoolContext();
            {
                //Check if there are any studentId in database 
                if (context.Students.Any())
                {
                    //Return highest studentid in students
                    return context.Students.Max(st => st.StudentId);
                }
                else
                {
                    //return 1 to avoid crash if Students is empty
                    return 1;
                }
            }
        }
        internal static int GetMaxSubjectId()
        {
            using var context = new Labb2SchoolContext();
            {
                if (context.Subjects.Any())
                {
                    return context.Subjects.Max(st => st.SubjectId);
                }
                else
                {
                    return 1;
                }
            }
        }
        internal static int GetMaxRoletId()
        {
            using var context = new Labb2SchoolContext();
            {
                if (context.Roles.Any())
                {
                    return context.Roles.Max(st => st.RoleId);
                }
                else
                {
                    return 1;
                }
            }
        }
        internal static int GetMaxDepartmentId()
        {
            using var context = new Labb2SchoolContext();
            {
                if (context.Departments.Any())
                {
                    return context.Departments.Max(st => st.DepartmentId);
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}

