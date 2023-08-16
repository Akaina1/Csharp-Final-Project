using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class ExpenseManger
    {
        public void ShowExpenses() 
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Expenses.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Expense>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"Description",-23} {"Amount",-15}");
                Console.WriteLine("--------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.Id,-5}\t{record.Description,-20}\t${record.Amount,-10:F2}");
                }
                Console.WriteLine("--------------------------------------");
            }
        }
    }
}