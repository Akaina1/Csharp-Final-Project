using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;


// the DatabaseCheck.cs is used to hold the Checker classes, they check each database CSV file to see if any conditions are met to create Notifications using AutoNotification() method.
namespace FinalProject
{
    public static class InventoryCheck
    {
        public static int MinInventory { get; set; }

        public static int MaxInventory { get; set; }

        public static void CheckInventory()
        {
            // check notifications.csv file for any notifications that have already been created

            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open inventory csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Inventory.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Product>().ToList(); // convert to list

                // check each record for conditions
                foreach (var record in records)
                {
                    if (record.InStock <= (MinInventory / 2) && record.OnOrder == 0 ) // very low inventory Urgent
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Very Low Inventory for: {record.Name}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Very Low Inventory for: {record.Name}", Notification.NotificationType.Urgent, Notification.Module.Inventory);
                        }
                    }

                   else if (record.InStock <= MinInventory && record.OnOrder == 0) // low inventory Warning
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Low Inventory for: {record.Name}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Low Inventory for: {record.Name}", Notification.NotificationType.Warning, Notification.Module.Inventory);
                        }    
                    }                  

                    else if (record.InStock >= MaxInventory) // high inventory
                    {
                        if (!existingNotifications.Any(n => n.Description == $"High Inventory for: {record.Name}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"High Inventory for: {record.Name}", Notification.NotificationType.Warning, Notification.Module.Inventory);
                        }
                    }
                }
            }
        }

        public static void SetMinInventory()
        {
            Console.WriteLine("Enter minimum inventory level: ");
            int minInventory = Convert.ToInt32(Console.ReadLine());

            // set InventoryCheck.minInventory to user input
            MinInventory = minInventory;

            Console.WriteLine("Minimum inventory level set to: " + minInventory);

            // delete all Low Inventory notifications
            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            // delete all Low Inventory notifications
            foreach (var notification in existingNotifications)
            {
                if (notification.Description.Contains("Low Inventory"))
                {
                    NotificationManager.DeleteNotification(notification.Id);
                }
            }

            // write new Options to Options.csv
            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Options.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(new List<Options> { new Options { UserMinInventory = minInventory, UserMaxInventory = MaxInventory, UserMarketingBudget = MarketingCheck.MarketingBudget, MarketingTotalCost = MarketingCheck.TotalCost  } });
            }
        }

        public static void SetMaxInventory()
        {
            Console.WriteLine("Enter maximum inventory level: ");
            int maxInventory = Convert.ToInt32(Console.ReadLine());

            // set InventoryCheck.maxInventory to user input
            MaxInventory = maxInventory;

            Console.WriteLine("Maximum inventory level set to: " + maxInventory);
            Console.WriteLine("Press any key to return to the Notification Manager Menu.");
            Console.ReadKey();

            // write new Options to Options.csv
            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Options.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(new List<Options> { new Options { UserMinInventory = MinInventory, UserMaxInventory = maxInventory, UserMarketingBudget = MarketingCheck.MarketingBudget, MarketingTotalCost = MarketingCheck.TotalCost } });
            }
        }
    }

    public static class ExpenseCheck
    {
        public static void CheckExpense()
        {
            // check notifications.csv file for any notifications that have already been created

            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open inventory csv file
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Expenses")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Expense>().ToList(); // convert to list

                // check each record for conditions
                foreach (var record in records)
                {
                    if (record.DueDate >= DateOnly.FromDateTime(DateTime.Today) && record.DueDate <= DateOnly.FromDateTime(DateTime.Today.AddDays(30))) // bill due within 30 days
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Expense due within 30 days: {record.Description}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Expense due within 30 days: {record.Description}", Notification.NotificationType.Reminder, Notification.Module.Expense);
                        }
                    }

                    else if (record.DueDate >= DateOnly.FromDateTime(DateTime.Today) && record.DueDate <= DateOnly.FromDateTime(DateTime.Today.AddDays(7))) // bill due within 7 days
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Expense due within 7 days: {record.Description}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Expense due within 7 days: {record.Description}", Notification.NotificationType.Urgent, Notification.Module.Expense);
                        }
                    } 
                }
            }
        }
    }

    public static class SalesCheck
    {
        public static void CheckSales()
        {
            // check notifications.csv file for any notifications that have already been created
            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open sales csv file
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Sales")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<SalesOrder>().ToList(); // convert to list

                // make unique list of sales orders by OrderId, each order should only appear once
                records = records.GroupBy(x => x.OrderId).Select(x => x.First()).ToList();

                // check each record for conditions
                foreach (var record in records)
                {
                    if (record.OrderStatus == SalesOrder.Status.Cancelled) // if SalesOrder.OrderStatus == Cancelled
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Sales Order Cancelled: {record.OrderId}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Sales Order Cancelled: {record.OrderId}", Notification.NotificationType.Urgent, Notification.Module.Sales);
                        }
                    }

                    if (record.OrderStatus == SalesOrder.Status.Delayed) // if SalesOrder.OrderStatus == Delayed
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Sales Order Delayed: {record.OrderId}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Sales Order Delayed: {record.OrderId}", Notification.NotificationType.Warning, Notification.Module.Sales);
                        }
                    }
                }
            }
        }
    }

    public static class OrderCheck
    {
        public static void CheckOrders()
        {
            // check notifications.csv file for any notifications that have already been created
            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open sales csv file
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Orders")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Order>().ToList(); // convert to list

                // check each record for conditions
                foreach (var record in records)
                {
                    if (record.OrderStatus == Order.Status.Cancelled) // if SalesOrder.OrderStatus == Cancelled
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Product Order Cancelled: {record.Id}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Product Order Cancelled: {record.Id}", Notification.NotificationType.Urgent, Notification.Module.Supplier);
                        }
                    }

                    if (record.OrderStatus == Order.Status.Delayed) // if SalesOrder.OrderStatus == Delayed
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Product Order Delayed: {record.Id}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Product Order Delayed: {record.Id}", Notification.NotificationType.Warning, Notification.Module.Supplier);
                        }
                    }
                }
            }
        }
    }

    public static class MarketingCheck
    {
        public static double MarketingBudget {get; set;}

        public static double TotalCost { get; set;}

        public static void SetMarketingBudget()
        {
            Console.WriteLine("Enter Marketing Budget ($): ");
            int newBudget = Convert.ToInt32(Console.ReadLine());
                       
            // delete over budget notifications
            List<Notification> existingNotifications; // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            // get only Marketing notifications
            existingNotifications = existingNotifications.Where(n => n.notificationModule == Notification.Module.Marketing).ToList();

            // delete marketing notifications
            foreach (var notification in existingNotifications)
            {
                if (notification.Description == $"Using 80%+ of Marketing Budget: ${TotalCost} / ${MarketingBudget}")
                {
                    // delete notification
                    NotificationManager.DeleteNotification(notification.Id);
                }

                if (notification.Description == $"Over Marketing Budget: {TotalCost} / {MarketingBudget}")
                {
                    // delete notification
                    NotificationManager.DeleteNotification(notification.Id);
                }
            } 
            
            // set MarketingBudget to user input
            MarketingBudget = newBudget;
            Console.WriteLine("Marketing budget set to: $" + newBudget);

            // write new Options to Options.csv
            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Options.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(new List<Options> { new Options { UserMinInventory = InventoryCheck.MinInventory, UserMaxInventory = InventoryCheck.MaxInventory, UserMarketingBudget = newBudget, MarketingTotalCost = TotalCost } });
            }
        }

        public static void CheckMarketing()
        {
            // check notifications.csv file for any notifications that have already been created

            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open Marketing csv file
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Marketing")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MarketingCampaign>().ToList(); // convert to list
               
                // check each record for conditions
                foreach (var record in records)
                {
                    if (record.EndDate >= DateOnly.FromDateTime(DateTime.Today) && record.EndDate <= DateOnly.FromDateTime(DateTime.Today.AddDays(30))) // marketing campaign ends in 30 days
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Marketing Campaign Ends in 30 days: {record.AdDetails}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Marketing Campaign Ends in 30 days: {record.AdDetails}", Notification.NotificationType.Reminder, Notification.Module.Marketing);
                        }
                    }

                    if (record.EndDate >= DateOnly.FromDateTime(DateTime.Today) && record.EndDate <= DateOnly.FromDateTime(DateTime.Today.AddDays(7))) // marketing campaign ends in 7 days
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Marketing Campaign Ends in 7 days: {record.AdDetails}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Marketing Campaign Ends in 7 days: {record.AdDetails}", Notification.NotificationType.Warning, Notification.Module.Marketing);
                        }
                    }
                }

                TotalCost = 0; // reset totalCost
                foreach (var campaign in records)
                {
                    TotalCost += campaign.Cost;
                }
                              
                // check if marketing campaign is close to budget, add Cost of every campaign in the Marketing.csv file
                if (TotalCost >= MarketingBudget) // if over budget
                {
                    if (!existingNotifications.Any(n => n.Description == $"Over Marketing Budget: {TotalCost} / {MarketingBudget}"))
                    {
                        // create notification
                        NotificationManager.AutoNotification($"Over Marketing Budget: {TotalCost} / {MarketingBudget}", Notification.NotificationType.Urgent, Notification.Module.Marketing);
                    }
                }

                if (TotalCost >= (MarketingBudget * 0.80)) // marketing campaign is within 20% of budget
                {
                    if (!existingNotifications.Any(n => n.Description == $"Using 80%+ of Marketing Budget: ${TotalCost} / ${MarketingBudget}"))
                    {
                        // create notification
                        NotificationManager.AutoNotification($"Using 80%+ of Marketing Budget: ${TotalCost} / ${MarketingBudget}", Notification.NotificationType.Warning, Notification.Module.Marketing);
                    }
                }
            }
        }
    }

    public static class OptionsCheck
    {
        public static void LoadUserOptions()
        {
            // open options CSV file and get values for MinInventory, MaxInventory, MarketingBudget, and TotalCost (marketing)
            // set each value to the corresponding variable upon loading the program
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Current Database","Options.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Options>().ToList(); // convert to list

                foreach (var record in records)
                {
                    InventoryCheck.MinInventory = record.UserMinInventory;
                    InventoryCheck.MaxInventory = record.UserMaxInventory;
                    MarketingCheck.MarketingBudget = record.UserMarketingBudget;
                    MarketingCheck.TotalCost = record.MarketingTotalCost;
                }
            }
        }
    }

    public static class FileManager
    {
        public static string GetCurrentDatabaseFile(string tableName)
        {
            return DateTime.Now.ToString("yyyy_MM_") + tableName + ".csv";
        }

        public static string GetFilePathForTable(string tableName)
        {
            string currentDatabase = GetCurrentDatabaseFile(tableName);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", currentDatabase);
            return path;
        }

        public static void CreateDatabaseFiles(string tableName)
        {
            string path = GetFilePathForTable(tableName);
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }

        public static void ArchiveLastMonthCSVs()
        {
            if (DateTime.Today.Day == GlobalState.ResetDay) //check if today is the day to reset the database
            {
                Console.WriteLine("Archiving last month's CSV files...");
                Console.ReadKey();

                string currentDatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database");

                // get current month's folder name
                DateTime currentMonth = DateTime.Now;
                string newMonthFolderName = currentMonth.ToString("yyyy_MM") + "_Database";
                string archiveDatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archived Databases", newMonthFolderName);

                // check if folder exists, if not create it
                if (!Directory.Exists(archiveDatabasePath))
                {
                    Directory.CreateDirectory(archiveDatabasePath);
                }

                // get file name prefix           
                DateTime nextMonth = currentMonth.AddMonths(1);
                string nextMonthPrefix = nextMonth.ToString("yyyy_MM_");

                CopyExpensesForNextMonth(currentDatabasePath, archiveDatabasePath, nextMonthPrefix);    
                CopyMarketingForNextMonth(currentDatabasePath, archiveDatabasePath, nextMonthPrefix);
                CopyOrdersForNextMonth(currentDatabasePath, archiveDatabasePath, nextMonthPrefix);
                CopySalesForNextMonth(currentDatabasePath, archiveDatabasePath, nextMonthPrefix);

                Console.WriteLine("Finished archiving last month's CSV files.");
                Console.ReadKey();
            
            }
        }

        public static void CopyExpensesForNextMonth(string currentDatabasePath, string archiveDatabasePath, string nextMonthPrefix)
        {
            // Define file paths
            string currentFilePath = Path.Combine(currentDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Expenses.csv");
            string nextMonthFilePath = Path.Combine(currentDatabasePath, nextMonthPrefix + "Expenses.csv");
            string archiveFilePath = Path.Combine(archiveDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Expenses.csv");

            // Read current month's data
            List<Expense> records;
            using (var reader = new StreamReader(currentFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                records = csv.GetRecords<Expense>().ToList();
            }

            // Filter records for the next month
            var nextMonth = DateTime.Now.AddMonths(1);
            var recordsForNextMonth = records.Where(r => r.DueDate.Month == nextMonth.Month && r.DueDate.Year == nextMonth.Year).ToList();

            // Write filtered records to next month's file
            using (var writer = new StreamWriter(nextMonthFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(recordsForNextMonth);
            }

            // Move current month's file to archive
            File.Move(currentFilePath, archiveFilePath);
        }

        public static void CopyMarketingForNextMonth(string currentDatabasePath, string archiveDatabasePath, string nextMonthPrefix)
        {
            // Define file paths
            string currentFilePath = Path.Combine(currentDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Marketing.csv");
            string nextMonthFilePath = Path.Combine(currentDatabasePath, nextMonthPrefix + "Marketing.csv");
            string archiveFilePath = Path.Combine(archiveDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Marketing.csv");

            // Read current month's data
            List<MarketingCampaign> records;
            using (var reader = new StreamReader(currentFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                records = csv.GetRecords<MarketingCampaign>().ToList();
            }

            // Filter records for the next month
            var nextMonth = DateTime.Now.AddMonths(1);
            var recordsForNextMonth = records.Where(r => r.EndDate.Month == nextMonth.Month && r.EndDate.Year == nextMonth.Year).ToList();

            // Write filtered records to next month's file
            using (var writer = new StreamWriter(nextMonthFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(recordsForNextMonth);
            }

            // Move current month's file to archive
            File.Move(currentFilePath, archiveFilePath);
        }

        public static void CopyOrdersForNextMonth(string currentDatabasePath, string archiveDatabasePath, string nextMonthPrefix)
        {
            // Define file paths
            string currentFilePath = Path.Combine(currentDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Orders.csv");
            string nextMonthFilePath = Path.Combine(currentDatabasePath, nextMonthPrefix + "Orders.csv");
            string archiveFilePath = Path.Combine(archiveDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Orders.csv");

            // Read current month's data
            List<Order> records;
            using (var reader = new StreamReader(currentFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                records = csv.GetRecords<Order>().ToList();
            }

            // Filter records for the next month
            var nextMonth = DateTime.Now.AddMonths(1);
            var recordsForNextMonth = records.Where(r => r.OrderStatus != Order.Status.Delivered).ToList();

            // Write filtered records to next month's file
            using (var writer = new StreamWriter(nextMonthFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(recordsForNextMonth);
            }

            // Move current month's file to archive
            File.Move(currentFilePath, archiveFilePath);
        }

        public static void CopySalesForNextMonth(string currentDatabasePath, string archiveDatabasePath, string nextMonthPrefix)
        {
            // Define file paths
            string currentFilePath = Path.Combine(currentDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Sales.csv");
            string nextMonthFilePath = Path.Combine(currentDatabasePath, nextMonthPrefix + "Sales.csv");
            string archiveFilePath = Path.Combine(archiveDatabasePath, DateTime.Now.ToString("yyyy_MM") + "_Sales.csv");

            // Read current month's data
            List<SalesOrder> records;
            using (var reader = new StreamReader(currentFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                records = csv.GetRecords<SalesOrder>().ToList();
            }

            // Filter records for the next month
            var nextMonth = DateTime.Now.AddMonths(1);
            var recordsForNextMonth = records.Where(r => r.OrderStatus != SalesOrder.Status.Delivered).ToList();

            // Write filtered records to next month's file
            using (var writer = new StreamWriter(nextMonthFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(recordsForNextMonth);
            }

            // Move current month's file to archive
            File.Move(currentFilePath, archiveFilePath);
        }
    }
}
