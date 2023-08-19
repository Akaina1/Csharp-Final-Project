using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class SupplierManager
    {
        public void ShowOrders()
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv")) // open file
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

            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv")) // open file
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
            using (var writer = new StreamWriter("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv", append: true)) // open file
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
            {
                if (new FileInfo("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv").Length == 0)
                {
                    csv.WriteHeader<Order>();
                    csv.NextRecord(); // Move to the next line after writing the header
                }

                csv.NextRecord();// Move to the next line after writing the record
                csv.WriteRecord(newOrder);

                writer.Flush(); // Ensure the data is written immediately
            }
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
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv")) // open file
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
                using (var writer = new StreamWriter("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv", append: false)) // open file
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
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
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