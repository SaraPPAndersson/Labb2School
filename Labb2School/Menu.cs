using System;
using Labb2School.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2School
{
    internal class Menu
    {
        internal static void MainMenu()
        {
            // Keeps the program running ans shows the main menu until user choose to exit
            bool active = true;
            while (active)
            {
                Console.WriteLine(" --- Huvud meny ---");
                Console.WriteLine("1. Visa elev");
                Console.WriteLine("2. Visa personal");
                Console.WriteLine("0. Avsluta");

                //Keeps asking for valid input. 
                int choice = HandleChoice(0, 2);
                Console.Clear();

                switch (choice)
                {
                    case 1:
                        StudentsMenu();
                        break;
                    case 2:
                        StaffMenu();
                        break;
                    case 0:
                        active = false;
                        break;
                }

            }
        }
        internal static void StudentsMenu()
        {
            bool running = true;
            while (running)
            {

                Console.WriteLine("--- Visa elev ---");
                Console.WriteLine("1. Sortera elever efter förnamn");
                Console.WriteLine("2. Sortera elever efter efternamn");
                Console.WriteLine("3. Visa elever i en viss klass");
                Console.WriteLine("4. Visa alla elevers information");
                Console.WriteLine("5. Visa aktiva kurser");
                Console.WriteLine("6. Visa betyg för elev");
                Console.WriteLine("7. Visa elev med ID");
                Console.WriteLine("0. Tillbaka");

                int choice = HandleChoice(0, 7);
                Console.Clear();

                switch (choice)
                {
                    case 1:
                        GetInfo.OrderByFirstName();
                        break;
                    case 2:
                        GetInfo.OrderByLastName();
                        break;
                    case 3:
                        MenuToChooseSpecificClass();
                        break;
                    case 4:
                        GetInfo.PrintAllStudentsInfo();
                        break;
                    case 5:
                        GetInfo.ShowActiveSubject();
                        Console.Clear();
                        break;
                    case 6:
                        {
                            ADO.ViewStudentsInfo();
                            Console.WriteLine("Skriv student Id:");
                            int studentId = int.Parse(Console.ReadLine());
                            Console.Clear();

                            ADO.ViewStudentsGrade(studentId);
                            break;
                        }
                    case 7:
                        {
                            Console.WriteLine("Skriv student Id:");
                            int studentId = int.Parse(Console.ReadLine());
                            ADO.PrintStudentById(studentId);
                            break;
                        }
                    case 0:
                        running = false;
                        break;
                }
            }
        }

        internal static bool StudentSortingMenu()
        {
            Console.WriteLine("1. Sortera efter stigande");
            Console.WriteLine("2. Sortera efter fallande");

            int choice = HandleChoice(1, 2);
            Console.Clear();
            //return true if coice is 1 true = 1 and false = 2 
            return choice == 1;
        }
        internal static void MenuToChooseSpecificClass()
        {
            //Show all classes with classId
            GetInfo.GetClasses();
            Console.WriteLine("Välj vilken klass du vill hämta informationen:");

            //Keeps asking for input until a valid classId is entered 
            int classId = HandleChoice(1, 6);
            //Send  the selected classId input to the method to get the students in the class
            GetInfo.GetStudentsByClass(classId);


        }
        internal static void StaffMenu()
        {
            bool running = true;
            while (running)
            {

                Console.WriteLine("--- Visa personal ---");
                Console.WriteLine("1. Visa personals info");
                Console.WriteLine("2. Visa lärare i avdelning");
                Console.WriteLine("3. Lägg till personal");
                Console.WriteLine("4. Visa lön per avdelning i månad");
                Console.WriteLine("5. Visa medellön per avdelning i månad");
                Console.WriteLine("0. Tillbaka");

                int choice = HandleChoice(0, 5);
                Console.Clear();

                switch (choice)
                {
                    case 1:
                       ADO.ViewStaff();
                        break;
                    case 2:
                        GetInfo.CountTeacherInDepartment();
                        break;
                    case 3:
                        ADO.InsertNewStaffInfo();
                        break;
                    case 4:
                        ADO.ViewSalaryPerDepartment();
                        break;
                    case 5:
                       ADO.AverageSalary();
                        break;
                    case 0:
                        running = false;
                        break;
                        
                }
            }
        }
        //To hande user's choice in range
        internal static int HandleChoice(int min, int max)
        {
            int userChoice = 0;
            bool validInput = false;

            while (!validInput)
            {
                if (!int.TryParse(Console.ReadLine(), out userChoice) || userChoice < min || userChoice > max)
                {
                    GetInfo.PrintErrorMessage();
                }
                else
                {
                    validInput = true;
                }
            }
            return userChoice;

        }
        internal static void ReadKeyAndClear()
        {
            Console.WriteLine("Tryck på en knapp...");
            Console.ReadKey();
            Console.Clear();
        }

    }
}
