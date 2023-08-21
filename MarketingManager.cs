using CsvHelper;
using System.Globalization;

namespace FinalProject
{
    public class MarketingManager
    {
        public void ShowCampaigns()
        {
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Marketing"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<MarketingCampaign>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Id",-7} {"Details",-31} {"Cost",-15} {"Views",-15} {"Clicks",-15} {"SalesFromAd",-15} {"StartDate",-15} {"EndDate"}");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    
                    Console.WriteLine($"{record.Id,-5}\t{record.AdDetails,-30}\t${record.Cost,-10:F2}\t{record.Views,-10}\t{record.Clicks,-10}\t{record.SalesFromAd,-10}\t{record.StartDate,-10}\t{record.EndDate,-10}");
                }
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            }
        }

        public void NewCampaign()
        {
            // get user input for new expense
            Console.WriteLine("Enter the marketing campaign details: ");
            string details = Console.ReadLine();

            // make sure description is not empty
            while (details == "")
            {
                Console.WriteLine("Details cannot be empty. Please enter details: ");
                details = Console.ReadLine();
            }

            Console.WriteLine("\nEnter the marketing campaign cost: ");
            // validate amount is a double
            double cost = Convert.ToDouble(Console.ReadLine());

            //get id of last entry in csv file
            int lastId = 0;

            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Marketing"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<MarketingCampaign>().ToList(); // convert to list

                if (records.Any())
                {
                    lastId = records.Last().Id;
                }
                else
                {
                    lastId = 500;
                }
            }

            // create new MarketingCampaign object
            MarketingCampaign newCampaign = new()
            {
                Id = lastId + 1,
                AdDetails = details,
                Cost = cost,
                Views = 0,
                Clicks = 0,
                SalesFromAd = 0
            };

            // append new record to csv file
            using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Marketing"), append: true)) // open file
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
            {
                if (new FileInfo(FileManager.GetFilePathForTable("Marketing")).Length == 0)
                {
                    csv.WriteHeader<MarketingCampaign>();
                    csv.NextRecord(); // Move to the next line after writing the header
                }

                csv.WriteRecord(newCampaign);

                writer.Flush(); // Ensure the data is written immediately
            }
        }

        public void DeleteCampaign()
        {
            // show all campaigns
            ShowCampaigns();

            // get user input for id of campaign to delete
            Console.WriteLine("Enter the id of the campaign to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            List<MarketingCampaign> records;

            // check if id exists in csv file
            using (var reader = new StreamReader(FileManager.GetFilePathForTable("Marketing"))) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                records = csv.GetRecords<MarketingCampaign>().ToList(); // convert to list
            }

            MarketingCampaign campaignToDelete = records.FirstOrDefault(x => x.Id == id);

            if (campaignToDelete != null)
            {
                // remove campaign from list
                records.Remove(campaignToDelete);

                // write list to csv file
                using (var writer = new StreamWriter(FileManager.GetFilePathForTable("Marketing"), append: false)) // open file
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
                {
                    csv.WriteHeader<MarketingCampaign>();
                    
                    foreach (var record in records)
                    {
                        csv.NextRecord();// Move to the next line after writing the record
                        csv.WriteRecord(record);
                    }

                    writer.Flush(); // Ensure the data is written immediately
                }

                Console.WriteLine("Campaign deleted successfully.");
                Console.WriteLine("Press any key to return to the Marketing Manager Menu.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("No campaign found with that id.");
                Console.WriteLine("Press any key to return to the Marketing Manager Menu.");
                Console.ReadKey();
            }
        }

        public void MarketingMenu()
        {
            Program.MenuHeader();
            NotificationManager.NotificationHeader();
            Console.WriteLine("Marketing Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Marketing Campaigns");
            Console.WriteLine("[2.]  New Marketing Campaign");
            Console.WriteLine("[3.]  Delete Marketing Campaign");
            Console.WriteLine("[4.]  Set Marketing Budget");
            Console.WriteLine("[5.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    ShowCampaigns();

                    Console.WriteLine("Press any key to return to the Marketing Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    MarketingMenu();
                    break;
                case 2:
                    Console.Clear();
                    NewCampaign();
                    Console.Clear();
                    MarketingMenu();
                    break;
                case 3:
                    Console.Clear();
                    DeleteCampaign();
                    Console.Clear();
                    MarketingMenu();
                    break;
                case 4:
                    Console.Clear();
                    MarketingCheck.SetMarketingBudget();
                    Console.Clear();
                    MarketingMenu();
                    break;
                case 5:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to Marketing Manager Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    MarketingMenu();
                    break;
            }
        }
    }
}