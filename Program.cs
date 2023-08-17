using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    internal class Program
    {
        static void Main()
        {
            // test inventory manager method
            //InventoryManager inventoryManager = new(); // can just use new() instead of new InventoryManager() to instantiate a new object - C# 9.0 feature
            // test supplier manager method
            //SupplierManager supplierManager = new();
            // test expense manager method
            //ExpenseManger expenseManger = new();
            // test sales manager method
            //SalesManager salesManager = new();

            //test add to inventory method - works
            //inventoryManager.AddToInventory(19051, "Apple", 1.99, 0.99, 70, 56);

            //inventoryManager.ShowInventory();
            //supplierManager.ShowOrders();
            //expenseManger.ShowExpenses();
            //salesManager.ShowSales();

            // creating a looping menu system that will allow the user to select which manager they want to use
            // and then which method they want to use
            // this will be done using a switch statement
            bool exit = false;

            do
            {
                Console.WriteLine("Welcome to the Inventory Management System!");
                Console.WriteLine("Please select which Module you would like to use:");
                Console.WriteLine("1. Inventory Manager");
                Console.WriteLine("2. Supplier Manager");
                Console.WriteLine("3. Expense Manager");
                Console.WriteLine("4. Sales Manager");
                Console.WriteLine("5. Exit");

                int choice = Convert.ToInt32(Console.ReadLine()); // convert user input to int

                // create switch statement
                switch (choice)
                {
                    case 1:
                        // go to inventory manager menu
                        InventoryManager inventoryManager = new();
                        inventoryManager.InventoryMenu();
                        break;
                    case 2:
                        // go to supplier manager menu
                        SupplierManager supplierManager = new();
                        supplierManager.SupplierMenu();
                        break;
                    case 3:
                        // go to expense manager menu
                        ExpenseManger expenseManger = new();
                        expenseManger.ExpenseMenu();
                        break;
                    case 4:
                        // go to sales manager menu
                        SalesManager salesManager = new();
                        salesManager.SalesMenu();
                        break;
                    case 5:
                        // exit program
                        exit = true; 
                        break;
                    default:
                        // invalid choice, return to main menu
                        Console.WriteLine("Invalid choice.");
                        exit = false;
                        break;
                } 

            } while (!exit);
        }
    }
}
