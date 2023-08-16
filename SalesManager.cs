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

                Console.WriteLine($"{"OrderId",-15} {"ProductName",-23} {"SalePrice",-15} {"Status",-15}");
                Console.WriteLine("---------------------------------------------------------------");
                for (int i = 0; i < records.Count; i++)
                {
                    if (i > 0 && records[i].OrderId != records[i - 1].OrderId)
                    {
                        Console.WriteLine("---------------------------------------------------------------");
                    }

                    var record = records[i];
                    Console.WriteLine($"{record.OrderId,-10}\t{record.ProductName,-20}\t${record.SalePrice,-10:F2}\t{record.OrderStatus,-20}");
                }
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
    }
}