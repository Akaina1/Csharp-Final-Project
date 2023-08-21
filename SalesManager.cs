using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class SalesManager
    {
        public void ShowSales()
        {
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Sales"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<SalesOrder>().ToList(); // convert to list

                // group records based on OrderId
                // use LINQ to group records by OrderId
                var groupedRecords = records.GroupBy(r => r.OrderId);

                // print out each record in formatted table to console
                Console.WriteLine($"{"OrderId",-23} {"ProductName",-15} {"SalePrice",-15} {"Status",-15}");
                Console.WriteLine("---------------------------------------------------------------");
                foreach (var group in groupedRecords)
                {
                    if (group.First().OrderStatus == SalesOrder.Status.Cancelled) { Console.ForegroundColor = ConsoleColor.Red; }
                    double totalCost = 0;
                    Console.WriteLine($"Id: {group.Key, -45}\t{group.First().OrderStatus}"); //group.First() is the first record in the group

                    foreach (var record in group)
                    {
                        totalCost += record.SalePrice;
                        Console.WriteLine($"\t\t\t{record.ProductName,-15}\t${record.SalePrice:F2}");

                    }

                    Console.WriteLine($"\n\nTotal Cost:  ${totalCost,5:F2}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("---------------------------------------------------------------");
                }
            }
        }

        public void NewSale()
        {
            List<SalesOrder> newOrders = new();

            // get last order id from csv file
            int lastId = 0;

            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Sales"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<SalesOrder>().ToList(); // convert to list

                if (records.Any())
                {
                    lastId = records.Last().OrderId;
                }
                else
                {
                    lastId = 1000;
                }
            }

            while (true)
            {
                Console.WriteLine("Enter a product name or type 'stop' to finish: ");
                string productName = Console.ReadLine();

                if (productName.ToLower() == "stop")
                    break;

                Console.WriteLine("Enter the sale price for " + productName + ": ");
                double salePrice = Convert.ToDouble(Console.ReadLine());

                newOrders.Add(new SalesOrder
                {
                    OrderId = lastId + 1,
                    ProductName = productName,
                    SalePrice = salePrice,
                    OrderStatus = SalesOrder.Status.Pending
                }); 

                // decrement the InStock value for the product by comparing productName to ProductName in Inventory.csv
                List<Product> currentInventory;

                using (var reader = new StreamReader(FileManager.GetFilePathForTable("Inventory")))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    currentInventory = csv.GetRecords<Product>().ToList();
                }

                var filteredInventory = currentInventory.Where(p => p.Name == productName);

                foreach (var product in filteredInventory)
                {
                    // if the product is out of stock, show warning message and exit function
                    if (product.InStock == 0)
                    {
                        Console.WriteLine("This product is out of stock. Please reorder");
                        return;
                    }
                    else  { product.InStock--; } 
                }

                // write new inventory to csv file
                using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Inventory"), append: true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(currentInventory);
                }
            }

            // append new orders to csv file
            using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Sales"), append: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                if (new FileInfo(FileManager.GetFilePathForTable("Sales")).Length == 0)
                {
                    csv.WriteHeader<SalesOrder>();
                }

                foreach (var order in newOrders)
                {
                    csv.NextRecord();
                    csv.WriteRecord(order);
                    
                    writer.Flush(); // Ensure the data is written immediately
                }
            }

            Console.WriteLine("Order added successfully!");

            // create Notification using AutoNotification() method
            NotificationManager.AutoNotification($"New Order Pending: #[{lastId + 1}]", Notification.NotificationType.Urgent,Notification.Module.Sales);
        }   

        public void DeleteSale()
        {
            // show all sales
            ShowSales();

            // get order id to delete
            Console.WriteLine("Enter the order id to delete: ");
            int orderToDelete = Convert.ToInt32(Console.ReadLine());

            // read all records from csv file
            List<SalesOrder> currentOrders;
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Sales")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                currentOrders = csv.GetRecords<SalesOrder>().ToList();
            }

            // filter out records with matching order id
            var filteredOrders = currentOrders.Where(o => o.OrderId != orderToDelete);

            foreach (var order in currentOrders)
            {
                if (order.OrderStatus == SalesOrder.Status.Cancelled)
                {
                    // if SalesOrder.OrderStatus == Cancelled, add stock back for each product in deleted order
                    List<Product> currentInventory;
                    using (var reader = new StreamReader(FileManager.GetFilePathForTable("Inventory")))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        currentInventory = csv.GetRecords<Product>().ToList();
                    }

                    var filteredInventory = currentInventory.Where(p => p.Name == currentOrders.Where(o => o.OrderId == orderToDelete).Select(o => o.ProductName).FirstOrDefault());

                    foreach (var product in filteredInventory)
                    {
                        product.InStock++;
                    }

                    // write filtered inventory to csv file
                    using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Inventory")))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteHeader<Product>();

                        foreach (var product in currentInventory)
                        {
                            csv.NextRecord();
                            csv.WriteRecord(product);
                        }
                    }
                }
            }

            // write filtered records to csv file
            using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Sales")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<SalesOrder>();
               
                foreach (var order in filteredOrders)
                {
                    csv.WriteRecord(order);
                    csv.NextRecord();
                }
            }

            Console.WriteLine($"Order: {orderToDelete}, was deleted successfully!");
            Console.WriteLine("Press any key to return to the Sales Manager Menu.");
            Console.ReadKey();
        }
        
        public void SalesMenu()
        {
            Program.MenuHeader();
            Console.WriteLine("Sales Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Sales");
            Console.WriteLine("[2.]  New Sale Entry");
            Console.WriteLine("[3.]  Delete Sale Entry");
            Console.WriteLine("[4.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
            case 1:
                Console.Clear();
                ShowSales();
                Console.WriteLine("Press any key to return to the Sales Manager Menu.");
                Console.ReadKey();
                Console.Clear();
                SalesMenu();
                break;
            case 2:
                Console.Clear();
                NewSale();
                Console.Clear();
                SalesMenu();
                break;
            case 3:
                if (GlobalState.CurrentUser?.adminLevel != User.AdminLevel.Admin)
                {
                    Console.Clear();
                    Console.WriteLine("You do not have access to delete sale entries, please contact an Administrator");
                    Console.WriteLine("Press any key to return to main menu.");
                    Console.ReadLine();
                    Console.Clear();
                    SalesMenu();   
                }
                else
                {
                    Console.Clear();
                    DeleteSale();
                    Console.Clear();
                    SalesMenu();
                }
                break;
            case 4:
                Console.Clear();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid choice.");
                Console.WriteLine("Press any key to return to Sales Manager Menu.");
                Console.ReadLine();
                Console.Clear();
                SalesMenu();
                break;
            }
        }
    }
}