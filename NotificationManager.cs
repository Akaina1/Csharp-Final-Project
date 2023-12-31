﻿using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class NotificationManager
    {
        public static void NotificationHeader() // static header to display notifications from the Notifications class
        {
            Console.WriteLine("Urgent Notifications: \n");

            // only first 3 Urgent notifications will be displayed
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Notification>().ToList(); // convert to list

                var urgentRecords = records.Where(x => x.notificationType == Notification.NotificationType.Urgent).Take(3); // get first 3 urgent records

                // print out each record in formatted table to console
                foreach (var record in urgentRecords)
                {
                    if (record.notificationType == Notification.NotificationType.Urgent)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{record.Date}] {record.Description}");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("--------------------------------------------------");

            // do database checks everytime the notification header is displayed
            InventoryCheck.CheckInventory();
            ExpenseCheck.CheckExpense();
            SalesCheck.CheckSales();
            OrderCheck.CheckOrders();
            MarketingCheck.CheckMarketing();
        }

        public void ShowNotifications()
        {
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Notification>().ToList(); // convert to list

                // sort records by NotificationType, Urgent > Warning > Reminder
                records = records.OrderBy(x => x.notificationType).ToList();

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id"} {"Description",16} {"Date",64} {"Type",15} {"Module",17}");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                foreach (var record in records) 
                {
                    if (record.notificationType == Notification.NotificationType.Urgent)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{record.Id}\t{record.Description,-65}\t{record.Date}\t{record.notificationType,-15}\t{record.notificationModule}");
                    }
                    else if (record.notificationType == Notification.NotificationType.Warning)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{record.Id}\t{record.Description,-65}\t{record.Date}\t{record.notificationType,-15}\t{record.notificationModule}");
                    }
                    else if (record.notificationType == Notification.NotificationType.Reminder)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{record.Id}\t{record.Description,-65}\t{record.Date}\t{record.notificationType,-15}\t{record.notificationModule}");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            }
        }

        public void NotificationMenu()
        {
            Program.MenuHeader();
            NotificationHeader();
            Console.WriteLine("Notification Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show all Notifications");
            Console.WriteLine("[2.]  PLACEHOLDER");
            Console.WriteLine("[3.]  Return to Main Menu");
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
                NotificationMenu();
            }

            switch (Choice)
            {
                case 1:
                    Console.Clear();
                    ShowNotifications();
                    Console.WriteLine("Press any key to return to the Notification Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    NotificationMenu();
                    break;
                case 2:
                    Console.Clear();
                    //OptionsMenu();
                    break;
                case 3:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to Notification Manager Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    NotificationMenu();
                    break;
            }
        }

        public static void AutoNotification(string description, Notification.NotificationType type ,Notification.Module module)
        {
            // get id of last notification in csv file
            int lastId = 0;

            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<Notification>().ToList(); // convert to list

                // if records is empty, set lastId to 900
                if (records.Count == 0)
                {
                    lastId = 900;
                }
                else
                lastId = records.Last().Id; // get id of last record
            }

            Notification newNotification = new()
            {
                Id = lastId + 1,
                Description = description,
                Date = DateOnly.FromDateTime(DateTime.Now),
                notificationType = type,
                notificationModule = module
            };

            // add notification to csv file
            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv"), append: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                if (new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")).Length == 0)
                {
                    csv.WriteHeader<Notification>();
                    csv.NextRecord();// Move to the next line after writing the record
                }

                csv.NextRecord();// Move to the next line after writing the record
                csv.WriteRecord(newNotification);
                writer.Flush(); // Ensure the data is written immediately
            }
        }

        public static void DeleteNotification(int id)
        {
            // delete notififcation with matching id
            var records = new List<Notification>();

            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                records = csv.GetRecords<Notification>().ToList(); // convert to list
            }

            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Current Database", "Notifications.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Notification>();
                csv.NextRecord();// Move to the next line after writing the record

                foreach (var record in records)
                {
                    if (record.Id != id)
                    {
                        csv.WriteRecord(record);
                        csv.NextRecord();// Move to the next line after writing the record
                    }
                }

                writer.Flush(); // Ensure the data is written immediately
            }
        }
    }  
}
