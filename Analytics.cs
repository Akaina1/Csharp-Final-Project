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
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
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

        }

        public static void MarketingAnalytics()
        {

        }

        public static void SalesAnalytics()
        {

        }

        public static void SupplierAnalytics()
        {

        }

        public static void FinancialAnalytics()
        {

        }
    }
}
