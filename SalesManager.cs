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

                // print out each record in formatted table to console

                Console.WriteLine($"{"OrderId",-7} {"ProductName",-23} {"SalePrice",-15} {"Status",-15}");
                Console.WriteLine("--------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.OrderId,-5}\t{record.ProductName,-20}\t${record.SalePrice,-10:F2}\t{record.OrderStatus,-20}");
                }
                Console.WriteLine("--------------------------------------");
            }
        }
    }
}