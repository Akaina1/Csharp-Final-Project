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
                    if (record.InStock <= MinInventory)
                    {
                        if (!existingNotifications.Any(n => n.Description == $"Low Inventory for: {record.Name}"))
                        {
                            // create notification
                            NotificationManager.AutoNotification($"Low Inventory for: {record.Name}", Notification.NotificationType.Urgent, Notification.Module.Inventory);
                        }    
                    }

                    if (record.InStock >= MaxInventory)
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
                    if (record.DueDate >= DateOnly.FromDateTime(DateTime.Today) && record.DueDate <= DateOnly.FromDateTime(DateTime.Today.AddDays(7)))
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

    }

    public static class UsersCheck
    {

    }

    public static class OrderCheck
    {

    }

    public static class MarketingCheck
    {

    }
}
