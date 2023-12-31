﻿using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class ExpenseManger
    {
        public void ShowExpenses() 
        {
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Expenses"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Expense>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"Description",-23} {"Amount",-15} {"Due Date"}");
                Console.WriteLine("--------------------------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.Id,-5}\t{record.Description,-20}\t${record.Amount,-10:F2}\t{record.DueDate}");
                }
                Console.WriteLine("--------------------------------------------------------");
            }
        }

        public void AddExpense()
        {
            // get user input for new expense
            // Id should be auto-incremented by 1 (last id + 1)
            Console.WriteLine("Enter the expense description: ");
            string description = Console.ReadLine();

            // make sure description is not empty
            while (description == "")
            {
                Console.WriteLine("Description cannot be empty. Please enter a description: ");
                description = Console.ReadLine();
            }

            Console.WriteLine("\nEnter the expense amount: ");
            // validate amount is a double
            double amount = Convert.ToDouble(Console.ReadLine());

            // get due date
            Console.WriteLine("\nEnter the expense due date (MM/DD/YYYY): ");
            DateTime dueDate = Convert.ToDateTime(Console.ReadLine());

            //get id of last entry in csv file
            int lastId = 0;

            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Expenses"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Expense>().ToList(); // convert to list

                if (records.Any())
                {
                    lastId = records.Last().Id;
                }
                else
                {
                    lastId = 200;
                }
            }

            // create new expense object
            Expense newExpense = new()
            {
                Id = lastId + 1,
                Description = description,
                Amount = amount,
                DueDate = DateOnly.FromDateTime(dueDate)
            };

            // add new expense to csv file
            using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Expenses"), append: true )) // open file and append to end
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
            {
                if (new FileInfo(FileManager.GetFilePathForTable("Expenses")).Length == 0)
                {
                    csv.WriteHeader<Expense>();
                    csv.NextRecord(); // Move to the next line after writing the header
                }

                csv.NextRecord();// Move to the next line after writing the record
                csv.WriteRecord(newExpense);

                writer.Flush(); // Ensure the data is written immediately
            }

        }

        public void DeleteExpense()
        {
            // show all expenses
            ShowExpenses();

            // get user input for id of expense to delete
            Console.WriteLine("Enter the id of the expense you would like to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            List<Expense> records;

            // check if id exists in csv file
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Expenses"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                records = csv.GetRecords<Expense>().ToList(); // convert to list
            }

            Expense expenseToDelete = records.FirstOrDefault(x => x.Id == id);

            if (expenseToDelete != null)
            {
                // delete expense from csv file
                records.Remove(expenseToDelete);

                // write updated list to csv file
                using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Expenses"), append: false)) // open file
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
                {
                    csv.WriteHeader<Expense>();
                    csv.NextRecord(); // Move to the next line after writing the header

                    foreach (var record in records)
                    {
                        csv.NextRecord();// Move to the next line after writing the record
                        csv.WriteRecord(record);
                    }

                    writer.Flush(); // Ensure the data is written immediately
                }

                Console.WriteLine("Expense deleted successfully.");
                Console.WriteLine("Press any key to return to the Expense Manager Menu.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("No expense found with that id.");
                Console.WriteLine("Press any key to return to the Expense Manager Menu.");
                Console.ReadKey();
            }
        }
    
        public void ExpenseMenu()
        {
            Program.MenuHeader();
            NotificationManager.NotificationHeader();
            Console.WriteLine("Expense Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Expenses");
            Console.WriteLine("[2.]  Add Expense");
            Console.WriteLine("[3.]  Delete Expense");
            Console.WriteLine("[4.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int Choice; // Convert user input to int

            // do error checking on userChoice, if invalid ask user to re-enter
            while (!int.TryParse(Console.ReadLine(), out Choice) || Choice < 0 || Choice > 6)
            {
                Console.Clear();
                Console.WriteLine("Invalid choice.");
                Console.WriteLine("Press any key to return to previous menu.");
                Console.ReadLine();
                Console.Clear();
                ExpenseMenu();
            }

            switch (Choice)
            {
                case 1:
                    Console.Clear();
                    ShowExpenses();

                    Console.WriteLine("Press any key to return to the Expense Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    ExpenseMenu();
                    break;
                case 2:
                    Console.Clear();
                    AddExpense();
                    Console.Clear();
                    ExpenseMenu();
                    break;
                case 3:
                    Console.Clear();
                    DeleteExpense();
                    Console.Clear();
                    ExpenseMenu();
                    break;
                case 4:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to Expense Manager Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    ExpenseMenu();
                    break;
            }
        }
    }
}