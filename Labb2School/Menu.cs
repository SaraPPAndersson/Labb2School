using System;
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
            Console.WriteLine("Välj meny");
            Console.WriteLine("1. Hämta elever sorterad efter förnamn.");
            Console.WriteLine("2. Hämta elever sorterad efter efternamn.");
            Console.WriteLine("3. Hämta alla elever i en viss klass");
            Console.WriteLine("4. Lägga till en ny personal");
            Console.WriteLine("5. Hämta alla personal");
            Console.WriteLine("0. Avsluta");

                //Keeps asking for valid input. 
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 5)
            {
                GetInfo.PrintErrorMessage();
            }
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
                     GetInfo.AddStaff();
                     break;
                case 5:
                     GetInfo.PrintStaffInfo();
                     break;
                case 0: 
                     active = false; //Stop the main menu loop and exist the program
                     Console.WriteLine("Avsluta programmet");
                     break;
            }

            }
        }
        internal static bool StudentMenu()
        {
            Console.WriteLine("1. Sortera efter stigande");
            Console.WriteLine("2. Sortera efter fallande");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 2)
            {
                GetInfo.PrintErrorMessage();
            }
            Console.Clear();
            //return true if coice is 1 true = 1 and false = 2 
            return choice == 1;
        }
        internal static void MenuToChooseSpecificClass()
        {
            int classId;
            bool validInput = false;

            //Show all classes with classId
            GetInfo.GetClasses();
            Console.WriteLine("Välj vilken klass du vill hämta informationen:");

            //Keeps asking for input until a valid classId is entered 
            while (!validInput) 
                if (!int.TryParse(Console.ReadLine(), out classId) || classId < 1 || classId > 6)
                {
                    GetInfo.PrintErrorMessage();
                    continue; //go back and ask again
                }
                else
                {
                    //Correct input 
                    validInput = true;
                    //Send  the selected classId input to the method to get the students in the class
                    GetInfo.GetStudentsByClass(classId);
                }
            }
        }
    }
}
