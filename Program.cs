using System.Net.NetworkInformation;

namespace FinalProject
{
    internal partial class Program
    {
        enum MenuActionResult
        {
            Continue,
            Logout,
            Exit,
        }
        public static void MenuHeader() // Create a header for the menu that will display the current users name and admin level followed by the date and time, then a line break
        {
            Console.WriteLine($"Current User: {GlobalState.CurrentUser.Name}");
            Console.WriteLine($"Admin Level: {GlobalState.CurrentUser.adminLevel}");
            Console.WriteLine($"Date: {DateTime.Now}");
            Console.WriteLine("--------------------------------------------------");
        }
        private static void DisplayMenu()
        {
            // Print header
            Program.MenuHeader();
            Console.WriteLine("Welcome to the Inventory Management System!\n");
            Console.WriteLine("Please select which Module you would like to use.\n");
            Console.WriteLine("--------------------------------------------------");
            if (GlobalState.CurrentUser?.adminLevel == User.AdminLevel.Admin || GlobalState.CurrentUser?.adminLevel == User.AdminLevel.Manager || GlobalState.CurrentUser?.adminLevel == User.AdminLevel.Employee)
            {
                Console.WriteLine("[1.]  Inventory Manager"); // all employees can use the inventory manager
                Console.WriteLine("[2.]  Sales Manager"); // all employees can use the sales manager
            }

            if (GlobalState.CurrentUser?.adminLevel == User.AdminLevel.Admin || GlobalState.CurrentUser?.adminLevel == User.AdminLevel.Manager)
            {
                Console.WriteLine("[3.]  Supplier Manager"); // only managers and admins can use the supplier manager

            }

            if (GlobalState.CurrentUser?.adminLevel == User.AdminLevel.Admin)
            {
                Console.WriteLine("[4.]  User Manager"); // only admins can use the user manager
                Console.WriteLine("[5.]  Expense Manager"); // only admins can use the expense manager
                Console.WriteLine("[6.]  Marketing Manager"); // only admins can use the marketing manager
            }
            Console.WriteLine("[7.]  Logout"); // all users can logout
            Console.WriteLine("[8.]  Exit Program"); // all users can exit the program
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Enter choice: ");
        } // display the menu options
        private static MenuActionResult HandleChoices(int userChoice, InventoryManager inventoryManager, SalesManager salesManager, SupplierManager supplierManager, UserManager userManager, ExpenseManger expenseManger, MarketingManager marketingManager)
        {
            // Create switch statement
            int menuChoice = userChoice;

            switch (menuChoice)
            {
                case 1:
                    // Go to inventory manager menu
                    Console.Clear();
                    inventoryManager.InventoryMenu();
                    return MenuActionResult.Continue;
                case 2:
                    // Go to sales manager menu
                    Console.Clear();
                    salesManager.SalesMenu();
                    return MenuActionResult.Continue;
                case 3:
                    // Go to supplier manager menu
                    if (GlobalState.CurrentUser?.adminLevel == User.AdminLevel.Employee)
                    {
                        Console.Clear();
                        Console.WriteLine("You do not have access to this module.");
                        Console.WriteLine("Press any key to return to main menu.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        supplierManager.SupplierMenu();
                    }
                    return MenuActionResult.Continue;
                case 4:
                    // Go to User manager menu
                    if (GlobalState.CurrentUser?.adminLevel != User.AdminLevel.Admin)
                    {
                        Console.Clear();
                        Console.WriteLine("You do not have access to this module.");
                        Console.WriteLine("Press any key to return to main menu.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        userManager.UserMenu();
                    }
                    return MenuActionResult.Continue;
                case 5:
                    // Go to expense manager menu
                    if (GlobalState.CurrentUser?.adminLevel != User.AdminLevel.Admin)
                    {
                        Console.Clear();
                        Console.WriteLine("You do not have access to this module.");
                        Console.WriteLine("Press any key to return to main menu.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        expenseManger.ExpenseMenu();
                    }
                    return MenuActionResult.Continue;
                case 6:
                    // Go to Marketing manager menu
                    if (GlobalState.CurrentUser?.adminLevel != User.AdminLevel.Admin)
                    {
                        Console.Clear();
                        Console.WriteLine("You do not have access to this module.");
                        Console.WriteLine("Press any key to return to main menu.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        marketingManager.MarketingMenu();
                    }
                    return MenuActionResult.Continue;
                case 7:
                    // Logout
                    Console.Clear();
                    Console.WriteLine("Logging you out...");
                    Console.ReadLine();
                    GlobalState.Logout();
                    return MenuActionResult.Logout;
                case 8:
                    // Exit program
                    Console.Clear();
                    Console.WriteLine("Exiting program...");
                    return MenuActionResult.Exit;
                default:
                    // Invalid choice, return to main menu
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to main menu.");
                    Console.ReadLine();
                    Console.Clear();
                    return MenuActionResult.Continue;
            }
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

            do
            { 
                // Allow the user to Login
                userManager.Login();

                if (GlobalState.CurrentUser == null)
                {
                    Console.WriteLine("Failed to Login.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    MenuActionResult result;

                    do
                    {
                        Console.Clear();
                        DisplayMenu(); // Display the menu options

                        int userChoice = Convert.ToInt32(Console.ReadLine()); // Convert user input to int
                        result = HandleChoices(userChoice, inventoryManager, salesManager, supplierManager, userManager, expenseManger, marketingManager); // Handle the user choice

                        if (result == MenuActionResult.Exit)
                        {
                            return;
                        }

                        if (result == MenuActionResult.Logout)
                        {
                            break;
                        }  

                    } while (result != MenuActionResult.Logout);

                }
            } while (true);
        }
    }
}
