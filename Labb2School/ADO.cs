using Labb2School.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Labb2School
{
    internal class ADO
    {
        private static readonly string _connectionString =
         "Server=localhost;Database=Labb2School;Integrated Security=true;TrustServerCertificate=true;";

        internal static void ViewStaff()
        {
            string query =
                    "SELECT " +
                    "s.StaffId, " +
                    "s.FirstName + ' ' + s.LastName AS Name, " +
                    "r.RoleName AS Position, " +
                    "d.DepartmentName AS Department, " +
                    "s.Salary, " +
                    "s.EmploymentDate " +
                    "FROM Staff s " +
                    "LEFT JOIN Roles r ON s.RoleId = r.RoleId " +
                    "LEFT JOIN Departments d ON s.DepartmentId = d.DepartmentId";

            ExecuteQuery(query);
        }

        internal static void ViewStudentsInfo()
        {
            string query = "SELECT StudentId, FirstName, LastName " +
                "FROM Students " +
                "ORDER BY StudentId";

            ExecuteQuery(query);
        }

        internal static void ViewStudentsGrade(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                        SELECT 
                        g.Grade,
                        st.FirstName + ' ' + st.LastName AS StudentName,
                        sub.SubjectName AS Subject,
                        sub.CourseCode AS CourseCode,
                        s.FirstName + ' ' + s.LastName AS TeacherName
                        FROM Students st
                        LEFT JOIN Grades g ON st.StudentId = g.StudentId
                        LEFT JOIN Subjects sub ON g.SubjectId = sub.SubjectId
                        LEFT JOIN Staff s ON g.StaffId = s.StaffId
                        WHERE st.StudentId = @StudentId
                        ORDER BY g.GradeDate DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;

                    connection.Open();

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Den här eleven har inga betyg ännu.");
                                return;
                            }
                            for (int i = 0; i < reader.FieldCount; i++)
                                Console.Write(reader.GetName(i) + "\t");
                            Console.WriteLine();

                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                    Console.Write($"{reader.GetValue(i)}\t");
                                Console.WriteLine();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SQL FEL: " + ex.Message);
                    }
                }
            }
        }
        internal static void ViewSalaryPerDepartment()
        {
            string query = @"
                    SELECT 
                    d.DepartmentName AS Department,
                    SUM(s.Salary) AS TotalSalaryPerMonth
                    FROM Staff s
                    JOIN Departments d ON s.DepartmentId = d.DepartmentId
                    GROUP BY d.DepartmentName
                    ORDER BY d.DepartmentName;
                    ";

            ExecuteQuery(query);
        }
        internal static void AverageSalary()
        {
            string query = @"
                    SELECT 
                    d.DepartmentName AS Department,
                    CAST(AVG(s.Salary) AS DECIMAL(10,2)) AS AverageSalary
                    FROM Staff s
                    JOIN Departments d ON s.DepartmentId = d.DepartmentId
                    GROUP BY d.DepartmentName
                    ORDER BY d.DepartmentName;
                    ";

            ExecuteQuery(query);
        }
        public static void ExecuteQuery(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                try
                {

                    using (var reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetName(i) + "\t");
                        }
                        Console.WriteLine();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetValue(i)}\t");
                            }
                            Console.WriteLine();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }
        }

        internal static void AddNewStaff(string FirstName, string LastName, int RoleId, int DepartmentId, decimal Salary, DateOnly EmploymentDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Staff (FirstName, LastName, RoleId, DepartmentId, Salary, EmploymentDate)
                         VALUES (@FirstName, @LastName, @RoleId, @DepartmentId, @Salary, @EmploymentDate)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@RoleId", RoleId);
                    command.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                    command.Parameters.AddWithValue("@Salary", Salary);
                    command.Parameters.AddWithValue("@EmploymentDate", EmploymentDate);
                    
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Ny personal har lagt till");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    connection.Close();
                }
            }
        }
        internal static void InsertNewStaffInfo()
        {
            Console.WriteLine("Lägga till en ny personal");
            Console.WriteLine("----------------------------");

            Console.WriteLine("Förnamn: ");
            string FirstName = Console.ReadLine() ?? "";

            Console.WriteLine("Efternamn: ");
            string LastName = Console.ReadLine() ?? "";

            PrintRolesInfo();
            Console.WriteLine("Position: ");
            int RoleId = int.Parse(Console.ReadLine());

            Console.WriteLine("Avdelning: ");
            int DepartmentId = int.Parse(Console.ReadLine());

            Console.WriteLine("Lön: ");
            decimal Salary = decimal.Parse(Console.ReadLine());

            Console.WriteLine("anställningsdatum: ");
            DateOnly EmploymentDate = DateOnly.Parse(Console.ReadLine());

            AddNewStaff(FirstName, LastName, RoleId, DepartmentId, Salary, EmploymentDate);
        }
        internal static void PrintRolesInfo()
        {
            string query = @"
                    SELECT  
                    d.DepartmentId,
                    d.DepartmentName,
                    r.RoleId, 
                    r.RoleName
                    FROM Staff s
                    RIGHT JOIN Departments d ON s.DepartmentId = d.DepartmentId
                    RIGHT JOIN Roles r ON s.RoleId = r.RoleId
                    ";

            ExecuteQuery(query);
        }
    }
}
