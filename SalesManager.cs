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
    }
}