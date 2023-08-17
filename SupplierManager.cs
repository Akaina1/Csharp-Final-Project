using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class SupplierManager
    {
        public void ShowOrders()
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Order>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"SupplierName",-15} {"ProductName",-23} {"AmountPerUnit",-15} {"Units",-15} {"TotalCost",-15} {"Status",-10}");
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.Id,-5}\t{record.SupplierName,-15}\t{record.ProductName,-16}\t{record.AmountPerUnit,-15:F2}\t{record.Units,-8}\t{record.TotalCost,-13}\t{record.OrderStatus,-13}");
                }
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
            }
        }

        public void NewSupplier()
        {

        }

        public void DeleteSupplier()
        {

        }

        public void SupplierMenu()
        {
            Console.WriteLine("Supplier Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Supplier Orders");
            Console.WriteLine("[2.]  New Supplier");
            Console.WriteLine("[3.]  Delete Supplier");
            Console.WriteLine("[4.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
            case 1:
                Console.Clear();
                ShowOrders();

                Console.WriteLine("Press any key to return to the Supplier Manager Menu.");
                Console.ReadKey();
                Console.Clear();
                SupplierMenu();
                break;
            case 2:
                Console.Clear();
                NewSupplier();
                SupplierMenu();
                break;
            case 3:
                Console.Clear();
                DeleteSupplier();
                SupplierMenu();
                break;
            case 4:
                Console.Clear();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                SupplierMenu();
                break;
            }
        }
    }
}