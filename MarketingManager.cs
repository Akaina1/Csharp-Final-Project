using CsvHelper;
using System.Globalization;
using static FinalProject.Program;

namespace FinalProject
{
    public class MarketingManager
    {
        public void ShowCampaigns()
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Marketing.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<MarketingCampaign>().ToList(); // convert to list

                // print out each record in formatted table to console

                Console.WriteLine($"{"Details",-30} {"Cost",-15} {"Views",-15} {"Clicks",-15} {"SalesFromAd",-15}");
                Console.WriteLine("------------------------------------------------------------------------------------------");
                foreach (var record in records)
                {
                    // use getters to access private fields in Product class
                    Console.WriteLine($"{record.AdDetails,-30}\t${record.Cost,-10:F2}\t{record.Views,-10}\t{record.Clicks,-10}\t{record.SalesFromAd,-10}");
                }
                Console.WriteLine("------------------------------------------------------------------------------------------");
            }
        }

        public void NewCampaign()
        {

        }

        public void DeleteCampaign()
        {

        }

        public void MarketingMenu()
        {
            Program.MenuHeader();
            Console.WriteLine("Marketing Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show Marketing Campaigns");
            Console.WriteLine("[2.]  New Marketing Campaign");
            Console.WriteLine("[3.]  Delete Marketing Campaign");
            Console.WriteLine("[4.]  Return to Main Menu");
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