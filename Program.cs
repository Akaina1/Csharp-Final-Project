namespace FinalProject
{
    internal partial class Program
    {
        public static void MenuHeader() // Create a header for the menu that will display the current users name and admin level followed by the date and time, then a line break
        {
            Console.WriteLine($"Current User: {GlobalState.CurrentUser.Name}");
            Console.WriteLine($"Admin Level: {GlobalState.CurrentUser.adminLevel}");
            Console.WriteLine($"Date: {DateTime.Now}");
            Console.WriteLine("--------------------------------------------------");
        }
        static void Main()
        {
            // Instantiate Manager Objects
            SalesManager salesManager = new();
            ExpenseManger expenseManger = new();
            SupplierManager supplierManager = new();
            InventoryManager inventoryManager = new();
            MarketingManager marketingManager = new();
            UserManager userManager = new();
            
            // Creating a looping menu system that will allow the user to select which manager they want to use
            bool exitProgram = false;

            do
            {
                // Allow the user to Login
                userManager.Login();

                if(GlobalState.CurrentUser == null)
                {
                    Console.WriteLine("Failed to Login.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadLine();
                    Console.Clear();
                }

                bool exitMenu = false;
                       
                do
                {
                    // Print header
                    Program.MenuHeader();
                    Console.WriteLine("Welcome to the Inventory Management System!\n");
                    Console.WriteLine("Please select which Module you would like to use.\n");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("[1.]  Inventory Manager");
                    Console.WriteLine("[2.]  Supplier Manager");
                    Console.WriteLine("[3.]  Expense Manager");
                    Console.WriteLine("[4.]  Sales Manager");
                    Console.WriteLine("[5.]  Marketing Manager");
                    Console.WriteLine("[6.]  Logout");
                    Console.WriteLine("[7.]  Exit Program");
                    Console.WriteLine("--------------------------------------------------");
                    Console.Write("Enter choice: ");

                    int choice = Convert.ToInt32(Console.ReadLine()); // Convert user input to int

                    // Create switch statement
                    switch (choice)
                    {
                    case 1:
                        // Go to inventory manager menu
                        Console.Clear();
                        inventoryManager.InventoryMenu();
                        break;
                    case 2:
                        // Go to supplier manager menu
                        Console.Clear();
                        supplierManager.SupplierMenu();
                        break;
                    case 3:
                        // Go to expense manager menu
                        Console.Clear();
                        expenseManger.ExpenseMenu();
                        break;
                    case 4:
                        // Go to sales manager menu
                        Console.Clear();
                        salesManager.SalesMenu();
                        break;
                    case 5:
                        // Go to Marketing manager menu
                        Console.Clear();
                        marketingManager.MarketingMenu();
                        break;
                    case 6:
                        // Logout
                        Console.Clear();
                        Console.WriteLine("Logging you out...");
                        Console.Clear();
                        Console.ReadLine();
                        GlobalState.Logout();
                        exitMenu = true;
                        break;
                    case 7:
                        // Exit program
                        Console.Clear();
                        Console.WriteLine("Exiting program...");
                        exitProgram = true;
                        exitMenu = true;
                        break;
                    default:
                        // Invalid choice, return to main menu
                        Console.Clear();
                        Console.WriteLine("Invalid choice.");
                        Console.WriteLine("Press any key to return to main menu.");
                        Console.ReadLine();
                        Console.Clear();
                        exitProgram = false;
                        break;
                    }
                } while (!exitMenu);
            } while (!exitProgram);
        }
    }
}
