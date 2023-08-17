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
                SoldLastMonth = soldLastMonth
            };

            // check if item already exists in inventory by comparing item id to existing item ids
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Inventory.csv")) // open file

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
            using (var writer = new StreamWriter("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Inventory.csv", append: true)) // open file
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
            {
                if (new FileInfo("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Inventory.csv").Length == 0)
                {
                    csv.WriteHeader<Product>();
                    csv.NextRecord(); // Move to the next line after writing the header
                }

                csv.NextRecord();// Move to the next line after writing the record
                csv.WriteRecord(product);

                writer.Flush(); // Ensure the data is written immediately
            }
        }

        public void DeleteInventoryItem()
        {

        }

        public void InventoryMenu()
        {
            Console.WriteLine("Inventory Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Inventory");
            Console.WriteLine("[2.]  Add Inventory Item");
            Console.WriteLine("[3.]  Delete Inventory Item");
            Console.WriteLine("[4.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    ShowInventory();

                    Console.WriteLine("Press any key to return to the Expense Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    InventoryMenu();
                    break;
                case 2:
                    Console.Clear();
                    AddToInventory();
                    InventoryMenu();
                    break;
                case 3:
                    Console.Clear();
                    //DeleteInventoryItem();
                    InventoryMenu();
                    break;
                case 4:
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    InventoryMenu();
                    break;
            }
        }
    }
}