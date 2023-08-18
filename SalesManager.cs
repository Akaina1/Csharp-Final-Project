using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class SalesManager
    {
        public void ShowSales()
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Sales.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<SalesOrder>().ToList(); // convert to list

                // group records based on OrderId
                // use LINQ to group records by OrderId
                var groupedRecords = records.GroupBy(r => r.OrderId);

                // print out each record in formatted table to console
                Console.WriteLine($"{"OrderId",-23} {"ProductName",-15} {"SalePrice",-15} {"Status",-15}");
                Console.WriteLine("---------------------------------------------------------------");
                foreach (var group in groupedRecords)
                {
                    double totalCost = 0;
                    Console.WriteLine($"Id: {group.Key, -45}\t{group.First().OrderStatus}"); //group.First() is the first record in the group

                    foreach (var record in group)
                    {
                        totalCost += record.SalePrice;
                        Console.WriteLine($"\t\t\t{record.ProductName,-15}\t${record.SalePrice:F2}");

                    }

                    Console.WriteLine($"\n\nTotal Cost:\t${totalCost,-10:F2}");
                    Console.WriteLine("---------------------------------------------------------------");
                }
            }
        }

        public void NewSale()
        {

        }

        public void DeleteSale()
        {

        }

        public void SalesMenu()
        {
            Program.MenuHeader();
            Console.WriteLine("Sales Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Sales");
            Console.WriteLine("[2.]  New Sale Entry");
            Console.WriteLine("[3.]  Delete Sale Entry");
            Console.WriteLine("[4.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
            case 1:
                Console.Clear();
                ShowSales();
                Console.WriteLine("Press any key to return to the Sales Manager Menu.");
                Console.ReadKey();
                Console.Clear();
                SalesMenu();
                break;
            case 2:
                Console.Clear();
                NewSale();
                Console.Clear();
                SalesMenu();
                break;
            case 3:
                if (GlobalState.CurrentUser?.adminLevel != User.AdminLevel.Admin)
                {
                    Console.Clear();
                    Console.WriteLine("You do not have access to delete sale entries, please contact an Administrator");
                    Console.WriteLine("Press any key to return to main menu.");
                    Console.ReadLine();
                    Console.Clear();
                    SalesMenu();   
                }
                else
                {
                    Console.Clear();
                    DeleteSale();
                    Console.Clear();
                    SalesMenu();
                }
                break;
            case 4:
                Console.Clear();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid choice.");
                Console.WriteLine("Press any key to return to Sales Manager Menu.");
                Console.ReadLine();
                Console.Clear();
                SalesMenu();
                break;
            }
        }
    }
}