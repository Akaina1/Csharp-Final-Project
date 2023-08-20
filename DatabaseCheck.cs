using CsvHelper;
using System.Globalization;


// the DatabaseCheck.cs is used to hold the Checker classes, they check each database CSV file to see if any conditions are met to create Notifications using AutoNotification() method.
namespace FinalProject
{
    public static class InventoryCheck
    {
        public static int MinInventory { get; set; } = 10; // minumum inventory level with default value of 10
        public static int MaxInventory { get; set; } = 100; // maximum inventory level with default value of 100
        public static void CheckInventory()
        {
            // check notifications.csv file for any notifications that have already been created

            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Notifications.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open inventory csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Inventory.csv"))
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
        }
    }

    public static class ExpenseCheck
    {
        public static void CheckExpense()
        {
            // check notifications.csv file for any notifications that have already been created

            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Notifications.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open inventory csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Expenses.csv"))
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
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Notifications.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open sales csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Sales.csv"))
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

    public static class UsersCheck
    {
        public static void CheckUsers()
        {

        }
    }

    public static class OrderCheck
    {
        public static void CheckOrders()
        {
            // check notifications.csv file for any notifications that have already been created
            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Notifications.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open sales csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Orders.csv"))
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
        public static double MarketingBudget {get; set;} = 200;
        public static double totalCost { get; set;} = 0;
        public static void SetMarketingBudget()
        {
            Console.WriteLine("Enter Marketing Budget ($): ");
            int newBudget = Convert.ToInt32(Console.ReadLine());
                       
            // delete over budget notifications
            List<Notification> existingNotifications; // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Notifications.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            // get only Marketing notifications
            existingNotifications = existingNotifications.Where(n => n.notificationModule == Notification.Module.Marketing).ToList();

            // check if new budget makes notifications invalid
            foreach (var notification in existingNotifications)
            {
                if (notification.Description == $"Using 80%+ of Marketing Budget: ${totalCost} / ${MarketingBudget}")
                {
                    // delete notification
                    NotificationManager.DeleteNotification(notification.Id);
                }

                if (notification.Description == $"Over Marketing Budget: {totalCost} / {MarketingBudget}")
                {
                    // delete notification
                    NotificationManager.DeleteNotification(notification.Id);
                }
            } 
            
            // set InventoryCheck.minInventory to user input
            MarketingBudget = newBudget;
            Console.WriteLine("Marketing budget set to: $" + newBudget);
        }
        public static void CheckMarketing()
        {
            // check notifications.csv file for any notifications that have already been created

            List<Notification> existingNotifications = new(); // create list of notifications

            // open notifications csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Notifications.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                existingNotifications = csv.GetRecords<Notification>().ToList();
            }

            //open Marketing csv file
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Marketing.csv"))
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

                totalCost = 0; // reset totalCost
                foreach (var campaign in records)
                {
                    totalCost += campaign.Cost;
                }
                              
                // check if marketing campaign is close to budget, add Cost of every campaign in the Marketing.csv file
                if (totalCost >= MarketingBudget) // if over budget
                {
                    if (!existingNotifications.Any(n => n.Description == $"Over Marketing Budget: {totalCost} / {MarketingBudget}"))
                    {
                        // create notification
                        NotificationManager.AutoNotification($"Over Marketing Budget: {totalCost} / {MarketingBudget}", Notification.NotificationType.Urgent, Notification.Module.Marketing);
                    }
                }

                if (totalCost >= (MarketingBudget * 0.80)) // marketing campaign is within 20% of budget
                {
                    if (!existingNotifications.Any(n => n.Description == $"Using 80%+ of Marketing Budget: ${totalCost} / ${MarketingBudget}"))
                    {
                        // create notification
                        NotificationManager.AutoNotification($"Using 80%+ of Marketing Budget: ${totalCost} / ${MarketingBudget}", Notification.NotificationType.Warning, Notification.Module.Marketing);
                    }
                }
            }
        }
    }
}
