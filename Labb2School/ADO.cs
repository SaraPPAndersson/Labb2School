using Labb2School.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Labb2School
{
    internal class ADO
    {
        private static readonly string _connectionString =
         "Server=localhost;Database=Labb2School;Integrated Security=true;TrustServerCertificate=true;";
        //View all staffs' info
        internal static void ViewStaff()
        {
            string query =
                    "SELECT " +
                    "s.StaffId AS [Personal Id], " +
                    "s.FirstName + ' ' + s.LastName AS Namn, " +
                    "r.RoleName AS Position, " +
                    "d.DepartmentName AS Avdelning, " +
                    "s.Salary, " +
                    "FORMAT(s.EmploymentDate, 'yyyy-MM-dd') AS Anställningsdatum " +
                    "FROM Staff s " +
                    "LEFT JOIN Roles r ON s.RoleId = r.RoleId " +
                    "LEFT JOIN Departments d ON s.DepartmentId = d.DepartmentId";

            ExecuteQuery(query);
            Menu.ReadKeyAndClear();
        }
        internal static void ViewStudentsInfo()
        {
            string query = "SELECT StudentId, FirstName, LastName " +
                "FROM Students " +
                "ORDER BY StudentId";

            ExecuteQuery(query);
        }
        //View students grade
        internal static void ViewStudentsGrade(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                        SELECT 
                        g.Grade,
                        st.FirstName + ' ' + st.LastName AS [Elev namn],
                        sub.SubjectName AS [Ämne],
                        sub.CourseCode AS [Kurskod],
                        s.FirstName + ' ' + s.LastName AS [Lärare],
                        FORMAT(g.GradeDate, 'yyyy-MM-dd') AS [Datum]
                        FROM Students st
                        LEFT JOIN Grades g ON st.StudentId = g.StudentId
                        LEFT JOIN Subjects sub ON g.SubjectId = sub.SubjectId
                        LEFT JOIN Staff s ON g.StaffId = s.StaffId
                        WHERE st.StudentId = @StudentId
                        ORDER BY g.GradeDate DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

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

                            int width = 20;

                            for (int i = 0; i < reader.FieldCount; i++)
                                Console.Write(reader.GetName(i).PadRight(width));
                            Console.WriteLine();
                            Console.WriteLine(new string('-', reader.FieldCount * width));

                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                    Console.Write(reader.GetValue(i).ToString().PadRight(width));
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
        //sum salary in each department
        internal static void ViewSalaryPerDepartment()
        {
            string query = @"
                    SELECT 
                    d.DepartmentName AS Avdelning,
                    SUM(s.Salary) AS [Total lön i månad]
                    FROM Staff s
                    JOIN Departments d ON s.DepartmentId = d.DepartmentId
                    GROUP BY d.DepartmentName
                    ORDER BY d.DepartmentName;
                    ";

            ExecuteQuery(query);
            Menu.ReadKeyAndClear();
        }
        //Calculate average salary 
        internal static void AverageSalary()
        {
            string query = @"
                    SELECT 
                    d.DepartmentName AS Avdelning,
                    CAST(AVG(s.Salary) AS DECIMAL(10,2)) AS Meddellön
                    FROM Staff s
                    JOIN Departments d ON s.DepartmentId = d.DepartmentId
                    GROUP BY d.DepartmentName
                    ORDER BY d.DepartmentName;
                    ";

            ExecuteQuery(query);
            Menu.ReadKeyAndClear();
        }
        //Execute and display the result in a table format
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
                        int columns = reader.FieldCount;

                        int width = 20;

                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write(reader.GetName(i).PadRight(width));
                        Console.WriteLine();
                        Console.WriteLine(new string('-', reader.FieldCount * width));

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                Console.Write(reader.GetValue(i).ToString().PadRight(width));
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
        //Add new staff 
        internal static void AddNewStaff(string FirstName, string LastName, int RoleId, int DepartmentId, decimal Salary, DateTime EmploymentDate)
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
                    command.Parameters.Add("@EmploymentDate", SqlDbType.DateTime).Value = EmploymentDate;

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Ny personal har lagt till");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    connection.Close();
                }
            }
        }
        //Input staff's info to Add new staff
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
            int RoleId = Menu.HandleChoice(1, GetInfo.GetMaxRoletId());

            Console.WriteLine("Avdelning: ");
            int DepartmentId = Menu.HandleChoice(1, GetInfo.GetMaxDepartmentId());

            Console.WriteLine("Lön: ");
            decimal Salary = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Anställningsdatum yyyy-mm-dd:  ");
            DateTime EmploymentDate = DateTime.Parse(Console.ReadLine());


            AddNewStaff(FirstName, LastName, RoleId, DepartmentId, Salary, EmploymentDate);
        }
        internal static void PrintRolesInfo()
        {
            string query = @"
                    SELECT  
                    d.DepartmentId AS [Avdelnings Id],
                    d.DepartmentName AS Avdelning ,
                    r.RoleId AS [Positions Id], 
                    r.RoleName AS [Position]
                    FROM Staff s
                    RIGHT JOIN Departments d ON s.DepartmentId = d.DepartmentId
                    RIGHT JOIN Roles r ON s.RoleId = r.RoleId
                    ";

            ExecuteQuery(query);
        }
        //Print student's info by choosing student's id
        internal static void PrintStudentById(int studentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("ShowStudentDetailsById", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("Den här eleven har inga uppgifter.");
                            return;
                        }

                        int width = 20;

                            for (int i = 0; i < reader.FieldCount; i++)
                                Console.Write(reader.GetName(i).PadRight(width));
                            Console.WriteLine();
                            Console.WriteLine(new string('-', reader.FieldCount * width));

                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                    Console.Write(reader.GetValue(i).ToString().PadRight(width));
                                Console.WriteLine();
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Menu.ReadKeyAndClear();
            }
        }
        //To revice input to set grade 
        internal static void InputGrade()
        {
            GetInfo.GetStudentsWithAndWithoutGrade();

            Console.WriteLine("Student Id: ");
            int studentId = Menu.HandleChoice(1, GetInfo.GetMaxStudentId());

            GetInfo.ShowActiveSubject();
            Console.WriteLine("Kurs Id: ");
            int subjectId = Menu.HandleChoice(1, GetInfo.GetMaxSubjectId());

            GetInfo.PrintTeachers();
            Console.WriteLine("Av lärare (Id): ");
            int staffId = int.Parse(Console.ReadLine());

            string grade;
            do
            {
                Console.WriteLine("Betyg (A-F): ");
                grade = Console.ReadLine().Trim().ToUpper();

            }
            while (grade.Length != 1 || !("ABCDEF".Contains(grade)));

            Console.WriteLine("Datum (yyyy-mm-dd)");
            DateTime gradeDate = DateTime.Parse(Console.ReadLine());

            SetGrade(grade, gradeDate, staffId, studentId, subjectId);
        }
        //Set grade on student with transaction
        internal static void SetGrade(string grade, DateTime gradeDate, int staffId, int studentId, int subjectId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                                UPDATE Grades
                                Set Grade = @Grade,
                                    GradeDate = @GradeDate,
                                    StaffId = @StaffId
                                WHERE StudentId = @StudentId
                                AND SubjectId = @SubjectId";

                        using (var command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@StudentId", studentId);
                            command.Parameters.AddWithValue("@SubjectId", subjectId);
                            command.Parameters.AddWithValue("@Grade", grade);
                            command.Parameters.AddWithValue("@StaffId", staffId);
                            command.Parameters.Add("@GradeDate", SqlDbType.DateTime).Value = gradeDate;

                            int rows = command.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                Console.WriteLine("Spara ändringen? (J = Ja, N = Nej): ");
                                string answer = Console.ReadLine().Trim().ToUpper();

                                if (answer == "J")
                                {
                                    transaction.Commit();
                                    Console.WriteLine("COMMIT - Betyg satts");
                                }
                                else
                                {
                                    transaction.Rollback();
                                    Console.WriteLine("ROLLBACK - Ändring ångrad.");
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                Console.WriteLine("Ingen rad uppdaterades");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("ROLLBACK");
                        Console.WriteLine(ex.Message);
                    }

                    Menu.ReadKeyAndClear();
                }
            }
        }

    }
}
