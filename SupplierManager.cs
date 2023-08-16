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
    }
}