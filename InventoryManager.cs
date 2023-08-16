using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class InventoryManager
    {
        public void ShowInventory() // read from csv file
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Inventory.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Product>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"Name",-23} {"Price",-15} {"Cost",-15} {"InStock",-15} {"SoldLastMonth",-10}");
                Console.WriteLine("---------------------------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.Id,-5}\t{record.Name,-20}\t${record.Price,-10:F2}\t${record.Cost,-7:F2}\t{record.InStock,-8}\t{record.SoldLastMonth,-13}");
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------");
            }
        }
    }
}