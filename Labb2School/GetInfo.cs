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
                bool IsAscending = Menu.StudentMenu();

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
            }
        }
        internal static void OrderByFirstName()
        {
            using (var context = new Labb2SchoolContext())
            {
                bool IsAscending = Menu.StudentMenu();

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
            }
        }
        internal static void PrintStudentInfo(List<Student> students)
        {
            //Recive a list and print 
            Console.WriteLine("Lista med alla studenter:\n");
            foreach (var student in students)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }
            Console.ReadKey();
            Console.Clear();
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
        internal static void GetClasses()
        {
            using (var context = new Labb2SchoolContext())
            {
                var printClasses = context.Classes.ToList();

                    foreach (var c in printClasses)
                    {
                        Console.WriteLine($"{c.ClassId}.  {c.ClassName} årskurs: {c.GradeLevel}");  
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
                string firstName = Console.ReadLine()?? "";

                Console.WriteLine("Efternamn: ");
                string lastName = Console.ReadLine()?? "";

                int roleId = 0;
                bool validInputId = false;
                while (!validInputId)
                {
                    Console.WriteLine("Avdelning: ");
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
                    Console.WriteLine($"{s.StaffId}. {s.Name},   Avdelning: {s.RoleName}");
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

    }
}
