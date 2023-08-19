using CsvHelper;
using System.Globalization;


// the DatabaseCheck class is used to check each database CSV file to see if any conditions are met to create Notifications using AutoNotification() method.
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
    }

    public static class ExpenseCheck
    {

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
