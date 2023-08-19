using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    internal partial class Program
    {
        public class NotificationManager
        {

            public void ShowNotifications()
            {
                using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Notifications.csv")) // open file
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
                {
                    var records = csv.GetRecords<Notification>().ToList(); // convert to list

                    // print out each record in formatted table to console

                    Console.WriteLine($"{"Description",-31} {"Date",-15} {"Type",-15} {"Module",-15}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------");
                    foreach (var record in records)
                    {
                        Console.WriteLine($"{record.Description,-5}\t{record.Date,-15}\t{record.notificationType,-10}\t{record.notificationModule,-10}");
                    }
                    Console.WriteLine("---------------------------------------------------------------------------------------------------");
                }
            }
            public void NotificationMenu()
            {
                Program.MenuHeader();
                Console.WriteLine("Notification Manager\n");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("[1.]  Show all Notifications");
                Console.WriteLine("[2.]  Add Notification");
                Console.WriteLine("[3.]  Delete Notification");
                Console.WriteLine("[4.]  Return to Main Menu");
                Console.WriteLine("--------------------------------------------------");

                Console.Write("Enter choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        ShowNotifications();

                        Console.WriteLine("Press any key to return to the Notification Manager Menu.");
                        Console.ReadKey();
                        Console.Clear();
                        NotificationMenu();
                        break;
                    case 2:
                        Console.Clear();
                        // NewNotification();
                        Console.Clear();
                        NotificationMenu();
                        break;
                    case 3:
                        Console.Clear();
                        // DeleteNotification();
                        Console.Clear();
                        NotificationMenu();
                        break;
                    case 4:
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice.");
                        Console.WriteLine("Press any key to return to Notification Manager Menu.");
                        Console.ReadLine();
                        Console.Clear();
                        NotificationMenu();
                        break;
                }
            }
        }
    }
}
