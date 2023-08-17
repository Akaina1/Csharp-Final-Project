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

        public void ExpenseMenu()
        {
            Console.WriteLine("1. Show Expenses");
            Console.WriteLine("2. Add Expense");
            Console.WriteLine("3. Delete Expense");
            Console.WriteLine("4. Return to Main Menu");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ShowExpenses();
                    ExpenseMenu();
                    break;
                case 2:
                    //AddExpense();
                    ExpenseMenu();
                    break;
                case 3:
                    //DeleteExpense();
                    ExpenseMenu();
                    break;
                case 4:
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    ExpenseMenu();
                    break;
            }
        }
    }
}