﻿using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class InventoryManager
    {
        public void ShowInventory() // read from csv file
        {
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Product>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"Name",-15} {"Price",-10} {"Cost",-12} {"InStock",-10} {"SoldThisMonth",-15} {"SoldLastMonth",-15} {"OnOrder"}");
                Console.WriteLine("--------------------------------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.Id}\t{record.Name}\t${record.Price:F2}\t   ${record.Cost:F2}\t{record.InStock}\t   {record.SoldThisMonth,-12}\t   {record.SoldLastMonth, -12}\t   {record.OnOrder}");
                }
                Console.WriteLine("--------------------------------------------------------------------------------------------------");
            }
        }

        public void AddToInventory() // add item to inventory
        {
            //get user input for new product
            Console.WriteLine("Enter the product id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nEnter the product name: ");
            string name = Console.ReadLine();

            Console.WriteLine("\nEnter the product price: ");
            double price = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("\nEnter the product cost: ");
            double cost = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("\nEnter the product quantity in stock: ");
            int inStock = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nEnter the product quantity sold last month: ");
            int soldLastMonth = Convert.ToInt32(Console.ReadLine());

            // create new product object
            Product product = new()
            {
                Id = id,
                Name = name,
                Price = price,
                Cost = cost,
                InStock = inStock,
                SoldThisMonth = 0,
                SoldLastMonth = soldLastMonth,
                OnOrder = 0
            };

            // check if item already exists in inventory by comparing item id to existing item ids
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Product>().ToList(); // convert to list

                foreach (var record in records)
                {
                    if (record.Id == product.Id)
                    {
                        Console.WriteLine("Item already exists in inventory.");
                        return;
                    }
                }
            }

            // add product to inventory
            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"), append: true)) // open file
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
            {
                if (new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inventory")).Length == 0)
                {
                    csv.WriteHeader<Product>();
                    csv.NextRecord(); // Move to the next line after writing the header
                }

                csv.WriteRecord(product);

                writer.Flush(); // Ensure the data is written immediately
            }
        }

        public void DeleteInventoryItem()
        {
            // show the current inventory
            ShowInventory();

            // get user input for item to delete
            Console.WriteLine("Enter the id of the item you would like to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            List<Product> records;

            // check if item exists in inventory by comparing item id to existing item ids
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                records = csv.GetRecords<Product>().ToList(); // convert to list
            }

            Product itemToDelete = records.FirstOrDefault(record => record.Id == id);

            if (itemToDelete != null)
            {
                // delete item from inventory
                records.Remove(itemToDelete);

                // write updated inventory to csv file
                using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"), append: false)) // open file
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
                {
                    csvWriter.WriteHeader<Product>();
                    
                    foreach (var item in records)
                    {
                    csvWriter.NextRecord();// Move to the next line after writing the record
                    csvWriter.WriteRecord(item);
                    }

                    writer.Flush(); // Ensure the data is written immediately
                }

                Console.WriteLine("Item deleted.");
                Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Item not found.");
                Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                Console.ReadKey();
            }
        }

        public void InventoryMenu()
        {
            Program.MenuHeader();
            NotificationManager.NotificationHeader();
            Console.WriteLine("Inventory Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Inventory");
            Console.WriteLine("[2.]  Add Inventory Item");
            Console.WriteLine("[3.]  Delete Inventory Item");
            Console.WriteLine("[4.]  Inventory Options");
            Console.WriteLine("[5.]  Return to Main Menu");
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
                InventoryMenu();
            }

            switch (Choice)
            {
                case 1:
                    Console.Clear();
                    ShowInventory();

                    Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    InventoryMenu();
                    break;
                case 2:
                    if (GlobalState.CurrentUser.adminLevel != User.AdminLevel.Admin)
                    {
                        Console.WriteLine("You do not have permission to add inventory items.");
                        Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                        Console.ReadKey();
                        Console.Clear();
                        InventoryMenu();
                    }
                    else
                    {
                        Console.Clear();
                        AddToInventory();
                        Console.Clear();
                        InventoryMenu();
                    }
                    break;
                case 3:
                    if (GlobalState.CurrentUser.adminLevel != User.AdminLevel.Admin)
                    {
                        Console.WriteLine("You do not have permission to delete inventory items.");
                        Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                        Console.ReadKey();
                        Console.Clear();
                        InventoryMenu();
                    }
                    else
                    {
                        Console.Clear();
                        DeleteInventoryItem();
                        Console.Clear();
                        InventoryMenu();
                    }
                    break;
                case 4:
                    if (GlobalState.CurrentUser.adminLevel != User.AdminLevel.Admin)
                    {
                        Console.WriteLine("You do not have permission to delete inventory items.");
                        Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                        Console.ReadKey();
                        Console.Clear();
                        InventoryMenu();
                    }
                    else
                    {
                        Console.Clear();
                        InventoryOptions();
                        Console.Clear();
                        InventoryMenu();
                    }
                    break;
                case 5:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to Inventory Manager Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    InventoryMenu();
                    break;
            }
        }

        public void InventoryOptions()
        {
            Program.MenuHeader();
            NotificationManager.NotificationHeader();
            Console.WriteLine("Inventory Options\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Change minumum inventory limit");
            Console.WriteLine("[2.]  Change maximum inventory limit");
            Console.WriteLine("[3.]  Return to Inventory Manager");
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
                InventoryOptions();
            }

            switch (Choice)
            {
                case 1:
                    Console.Clear();
                    InventoryCheck.SetMinInventory();
                    Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    InventoryMenu();
                    break;
                case 2:
                    Console.Clear();
                    InventoryCheck.SetMaxInventory();
                    Console.WriteLine("Press any key to return to the Inventory Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    InventoryMenu();
                    break;
                case 3:
                    Console.Clear();
                    InventoryMenu();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to Inventory Manager Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    InventoryMenu();
                    break;
            }
        }
    }
}