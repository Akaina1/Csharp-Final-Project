using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public static class Analytics
    {
        public static void AnalyticsMenu()
        {
            Program.MenuHeader();
            NotificationManager.NotificationHeader();
            Console.WriteLine("Analytics Dashboard\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Inventory Analytics");
            Console.WriteLine("[2.]  Marketing Analytics");
            Console.WriteLine("[3.]  Sales Analytics");
            Console.WriteLine("[4.]  Supplier Analytics");
            Console.WriteLine("[5.]  Financial Analytics");
            Console.WriteLine("[6.]  Return to Main Menu");
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
                AnalyticsMenu();
            }

            switch (Choice)
            {
                case 1:
                    Console.Clear();
                    InventoryAnalytics();

                    Console.WriteLine("Press any key to return to the Analytics Dashboard.");
                    Console.ReadKey();
                    Console.Clear();
                    AnalyticsMenu();
                    break;
                case 2:
                    Console.Clear();
                    MarketingAnalytics();

                    Console.WriteLine("Press any key to return to the Analytics Dashboard.");
                    Console.ReadKey();
                    Console.Clear();
                    AnalyticsMenu();
                    break;
                case 3:
                    Console.Clear();
                    SalesAnalytics();

                    Console.WriteLine("Press any key to return to the Analytics Dashboard.");
                    Console.ReadKey();
                    Console.Clear();
                    AnalyticsMenu();
                    break;
                case 4:
                    Console.Clear();
                    SupplierAnalytics();

                    Console.WriteLine("Press any key to return to the Analytics Dashboard.");
                    Console.ReadKey();
                    Console.Clear();
                    AnalyticsMenu();
                    break;
                case 5:
                    Console.Clear();
                    SupplierAnalytics();

                    Console.WriteLine("Press any key to return to the Analytics Dashboard.");
                    Console.ReadKey();
                    Console.Clear();
                    AnalyticsMenu();
                    break;
                case 6:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to the Analytics Dashboard.");
                    Console.ReadLine();
                    Console.Clear();
                    AnalyticsMenu();
                    break;
            }
        }

        public static void InventoryAnalytics()
        {
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
               
                var records = csv.GetRecords<Product>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"Name",-15} {"Price",-10} {"Cost",-12} {"InStock",-10} {"SoldThisMonth",-15} {"SoldLastMonth",-15} {"OnOrder",-10} {"GrossSales",-13} {"GrossCost",-13} {"NetProfit"}");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    var grossSales = 0.0;
                    var grossCost = 0.0;
                    var netSales = 0.0;

                    grossSales += record.Price * record.SoldThisMonth;
                    grossCost += record.Cost * record.SoldThisMonth;
                    netSales += (record.Price - record.Cost) * record.SoldThisMonth;
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.Id}\t{record.Name}\t${record.Price:F2}\t   ${record.Cost:F2}\t{record.InStock}\t   {record.SoldThisMonth,-12}\t   {record.SoldLastMonth,-12}\t   {record.OnOrder}\t      {grossSales:F2}\t    {grossCost,-13:F2} {netSales:F2}");
                }
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Total Inventory amount:   " + records.Sum(x => x.InStock).ToString("N0"));
                Console.WriteLine("Total Inventory value:   $" + records.Sum(x => x.InStock * x.Cost).ToString("N2"));

                Console.WriteLine("Total Sold this month:    " + records.Sum(x => x.SoldThisMonth).ToString("N0"));
                Console.WriteLine("Total Sold last month:    " + records.Sum(x => x.SoldLastMonth).ToString("N0"));
                
                Console.WriteLine("Monthly Gross Sales:     $" + records.Sum(x => x.Price * x.SoldThisMonth).ToString("N2"));
                Console.WriteLine("Monthly Gross Cost:      $" + records.Sum(x => x.Cost * x.SoldThisMonth).ToString("N2"));
                Console.WriteLine("Monthly Net Profit:      $" + records.Sum(x => (x.Price - x.Cost) * x.SoldThisMonth).ToString("N2"));
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");

            }
        }

        public static void MarketingAnalytics()
        {
            Console.WriteLine("Marketing Analytics Placeholder");
            // placeholder
        }

        public static void SalesAnalytics()
        {
            Console.WriteLine("Sales Analytics Placeholder");
            // placeholder
        }

        public static void SupplierAnalytics()
        {
            Console.WriteLine("Supplier Analytics Placeholder");
            // placeholder
        }

        public static void FinancialAnalytics()
        {
            Console.WriteLine("Financial Analytics Placeholder");
            // placeholder
        }
    }
}
