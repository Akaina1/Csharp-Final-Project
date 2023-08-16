using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class InventoryManager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; } // in Dollars
        public double Cost { get; set; } // in Dollars
        public int InStock { get; set; }
        public int SoldLastMonth { get; set; }

        public void ShowInventory() // read from csv file
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Inventory.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<InventoryManager>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine("Id\tName\t\t\tPrice\tCost\t\tInStock\tSoldLastMonth");
                Console.WriteLine("-----------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    Console.WriteLine($"{record.Id}\t{record.Name}\t\t${record.Price}\t${record.Cost}\t\t{record.InStock}\t{record.SoldLastMonth}");
                }
                Console.WriteLine("-----------------------------------------------------------------------------");
            }
        }
    }
}