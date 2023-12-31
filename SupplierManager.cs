﻿using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class SupplierManager
    {
        public void ShowOrders()
        {
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Orders"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Order>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"SupplierName",-15} {"ProductName",-23} {"AmountPerUnit",-15} {"Units",-15} {"TotalCost",-15} {"Status",-10}");
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.Id,-5}\t{record.SupplierName,-15}\t{record.ProductName,-16}\t{record.AmountPerUnit,-15:F2}\t{record.Units,-8}\t{record.TotalCost,-13}\t{record.OrderStatus,-13}");
                }
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
            }
        }

        public void NewOrder()
        {
            //get user input for new Order
            Console.WriteLine("Enter the Supplier Name: ");
            string supplierName = Console.ReadLine();

            Console.WriteLine("\nEnter the Product Name: ");
            string productName = Console.ReadLine();

            Console.WriteLine("\nEnter the Amount Per Unit: ");
            double amountPerUnit = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("\nEnter the Units: ");
            int units = Convert.ToInt32(Console.ReadLine());

            // total cost is calculated by multiplying amount per unit by units
            double totalCost = amountPerUnit * units;

            //get id of last entry in csv file
            int lastId = 0;

            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Orders"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Order>().ToList(); // convert to list

                if (records.Any())
                {
                    lastId = records.Last().Id;
                }
                else
                {
                    lastId = 6000;
                }
            }

            // create new Order object
            Order newOrder = new()
            {
                Id = lastId + 1,
                SupplierName = supplierName,
                ProductName = productName,
                AmountPerUnit = amountPerUnit,
                Units = units,
                TotalCost = totalCost,
                OrderStatus = Order.Status.Pending
            };

            // append new Order to csv file
            using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Orders"), append: true)) // open file
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
            {
                if (new FileInfo(FileManager.GetFilePathForTable("Orders")).Length == 0)
                {
                    csv.WriteHeader<Order>();
                    csv.NextRecord(); // Move to the next line after writing the header
                }

                csv.NextRecord();// Move to the next line after writing the record
                csv.WriteRecord(newOrder);

                writer.Flush(); // Ensure the data is written immediately
            }

            // check if there is a low stock notification for this product by seeing if productName exists in the Notification description string
            // only Notifications within the Inventory Module are checked {Notification.Module.Inventory}
            List<Notification> notifRecords;
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                notifRecords = csv.GetRecords<Notification>().ToList(); // convert to list
            }

            foreach (var record in notifRecords)
            {
                if (record.notificationModule == Notification.Module.Inventory && record.Description.Contains(productName))
                {
                    // if there is a low stock notification for this product, delete it
                    NotificationManager.DeleteNotification(record.Id);
                }
            }

            // add amount of units ordered to OnOrder field in Product.csv
            List<Product> productRecords;
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                productRecords = csv.GetRecords<Product>().ToList(); // convert to list
            }

            Product productToUpdate = productRecords.FirstOrDefault(x => x.Name == productName);
            if (productToUpdate != null) 
            {
                productToUpdate.OnOrder += units;
            }

            // write updated product to csv file
            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
            {
                csv.WriteRecords(productRecords);
            }

            Console.WriteLine("Order added successfully.");
            Console.WriteLine("Press any key to return to the Supplier Manager Menu.");
            Console.ReadKey();
        }

        public void DeleteOrder()
        {
            // show orders
            ShowOrders();

            //get user input for id of order to delete
            Console.WriteLine("Enter the Id of the Order to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            List<Order> records;

            // open csv file
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Orders"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                records = csv.GetRecords<Order>().ToList(); // convert to list
            }

            Order orderToDelete = records.FirstOrDefault(x => x.Id == id);

            if (orderToDelete != null)
            {
                // remove campaign from list
                records.Remove(orderToDelete);

                // write list to csv file
                using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Orders"), append: false)) // open file
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
                {
                    csv.WriteHeader<Order>();
                    
                    foreach (var record in records)
                    {
                        csv.NextRecord();// Move to the next line after writing the record
                        csv.WriteRecord(record);
                    }

                    writer.Flush(); // Ensure the data is written immediately
                }

                // subtract amount of units ordered from OnOrder field in Inventory.csv
                List<Product> productRecords;
                using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
                {
                    productRecords = csv.GetRecords<Product>().ToList(); // convert to list
                }

                Product productToUpdate = productRecords.FirstOrDefault(x => x.Name == orderToDelete.ProductName);

                if (productToUpdate != null)
                {
                    productToUpdate.OnOrder -= orderToDelete.Units;
                }

                using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
                {
                    csv.WriteRecords(productRecords);
                }

                Console.WriteLine("Order deleted successfully.");
                Console.WriteLine("Press any key to return to the Supplier Manager Menu.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("No order found with that id.");
                Console.WriteLine("Press any key to return to the Supplier Manager Menu.");
                Console.ReadKey();
            }
        }

        public void SupplierMenu()
        {
            Program.MenuHeader();
            NotificationManager.NotificationHeader();
            Console.WriteLine("Supplier Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Supplier Orders");
            Console.WriteLine("[2.]  New Order");
            Console.WriteLine("[3.]  Delete Order");
            Console.WriteLine("[4.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int Choice; // Convert user input to int

            // do error checking on userChoice, if invalid ask user to re-enter
            while (!int.TryParse(Console.ReadLine(), out Choice) || Choice < 0 || Choice > 9)
            {
                Console.Clear();
                Console.WriteLine("Invalid choice.");
                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadLine();
                Console.Clear();
                SupplierMenu();
            }

            switch (Choice)
            {
            case 1:
                Console.Clear();
                ShowOrders();

                Console.WriteLine("Press any key to return to the Supplier Manager Menu.");
                Console.ReadKey();
                Console.Clear();
                SupplierMenu();
                break;
            case 2:
                Console.Clear();
                NewOrder();
                Console.Clear();
                SupplierMenu();
                break;
            case 3:
                Console.Clear();
                DeleteOrder();
                Console.Clear();
                SupplierMenu();
                break;
            case 4:
                Console.Clear();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid choice.");
                Console.WriteLine("Press any key to return to Supplier Manager Menu.");
                Console.ReadLine();
                Console.Clear();
                SupplierMenu();
                break;
            }
        }
    }
}