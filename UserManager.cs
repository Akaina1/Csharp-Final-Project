using CsvHelper;
using System.Globalization;
// passwords are stored as plain text in the CSV file
// in a real application, passwords would be hashed and salted
// this is for demonstration purposes only
namespace FinalProject
{
    public class UserManager
    {
        public void LoginScreen(string username, string password)
        {
            //print formatted login screen to console
            Console.WriteLine("Welcome to the Inventory Management System!\n");
            Console.WriteLine("Please login to continue.");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Username: {username}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine("--------------------------------------------------");
        }

        public void Login()
        {
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Users.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<User>().ToList(); // convert to list

                //set variable for current user to be returned by method
                User currentUser = new();

                LoginScreen("", "");
                bool usernameExists = false;
                string username = "";
                string password = "";

                while (usernameExists == false)
                {
                    // get username from user
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();

                    // check if username exists in CSV file
                    usernameExists = records.Any(x => x.Name == username);

                    // if Username exists, break
                    if (usernameExists == true) { break; }

                    // if username doesn't exist, tell user to retype username
                    Console.Clear();
                    Console.WriteLine("Username does not exist. Please try again.");
                    Console.ReadLine();
                    Console.Clear();
                    LoginScreen("", "");
                }

                Console.Clear();
                LoginScreen(username, "");
                bool passwordMatches = false;

                while (passwordMatches == false)
                {
                    // get password from user
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();

                    // check if password matches username
                    passwordMatches = records.Any(x => x.Name == username && x.Password == password);

                    //if pasword matches, break
                    if (passwordMatches == true) { break; }

                    // if password doesn't match, tell user to retype password
                    Console.Clear();
                    Console.WriteLine("Password does not match username. Please try again.");
                    Console.ReadLine();
                    LoginScreen(username, "");
                }

                Console.Clear();
                LoginScreen(username, password);

                // if username and password match, tell user they are logged in
                //set current user to user that matches username and password
                currentUser = records.Find(x => x.Name == username && x.Password == password);

                Console.WriteLine("User Confirmed.\n");
                Console.WriteLine("You are logged in.\n");
                Console.WriteLine("Press any key to go to the Main Menu");
                Console.ReadLine();
                Console.Clear();
                GlobalState.CurrentUser = currentUser;
                return;
            }
        }

        public void AddUser()
        {
            // get input for new user
            Console.WriteLine("Please enter the following information for the new user.");
            Console.WriteLine("--------------------------------------------------------");
                       
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.Write("Admin Level: ");
            // convert AdminLevel string to enum
            User.AdminLevel adminLevel = (User.AdminLevel)Enum.Parse(typeof(User.AdminLevel), Console.ReadLine());

            // get last userId from csv file
            int lastId = 0;

            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Users.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<User>().ToList(); // convert to list

                if(records.Any())
                {
                    lastId = records.Last().UserId;
                }
                else
                {
                    lastId = 1000;
                }
            }

            // create new user object
            User newUser = new()
            {
                UserId = lastId + 1,
                Name = username,
                Password = password,
                adminLevel = adminLevel
            };

            // add to CSV file
            using (var writer = new StreamWriter("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Users.csv", append: true)) // open file
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // read file
            {
                if (new FileInfo("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Users.csv").Length == 0)
                {
                    csv.WriteHeader<User>();
                    csv.NextRecord(); // Move to the next line after writing the header
                }

                csv.NextRecord();// Move to the next line after writing the record
                csv.WriteRecord(newUser);

                writer.Flush(); // Ensure the data is written immediately
            }
        }

        public void ShowUsers()
        {
            // show all users in CSV file, only show username, id, and AdminLevel
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Users.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                var records = csv.GetRecords<User>().ToList(); // convert to list

                Console.WriteLine("All Users\n");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("UserId\tUsername\tAdminLevel");
                Console.WriteLine("--------------------------------------------------");

                foreach (var user in records)
                {
                    Console.WriteLine($"{user.UserId}\t{user.Name}\t\t{user.adminLevel}");
                }

                Console.WriteLine("--------------------------------------------------");
            }
        }

        public void DeleteUser()
        {
            // show users
            ShowUsers();

            // get user id to delete
            Console.Write("Enter UserId of user to delete: ");
            int userId = Convert.ToInt32(Console.ReadLine());

            List<User> records;

            // check if user exists
            using (var reader = new StreamReader("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Users.csv")) // open file
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // read file
            {
                records = csv.GetRecords<User>().ToList(); // convert to list
            }

            User userToDelete = records.FirstOrDefault(x => x.UserId == userId);

            // ask user to confirm deletion by entering their password
            Console.Write($"Please enter your password to confirm deletion of {userToDelete.Name}: ");
            string password = Console.ReadLine();

            // check if password matches by comparing global current user password to entered password stored in User CSV file
            if (password != GlobalState.CurrentUser.Password)
            {
                Console.WriteLine("Password does not match. Please try again.");
                Console.ReadLine();
                Console.Clear();
                DeleteUser();
            }

            if (userToDelete != null)
            { 
                //remove user from list
                records.Remove(userToDelete);

                // write list to CSV file
                using (var writer = new StreamWriter("D:\\School\\School Work Code\\Udemy Code\\(3) C# Advanced Topics\\C# Final Project\\FinalProject\\Database\\Users.csv")) // open file
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) // write to file
                {
                    csv.WriteRecords(records);

                    foreach (var user in records)
                    {
                        csv.NextRecord();// Move to the next line after writing the record
                        csv.WriteRecord(user);
                    }

                    writer.Flush(); // Ensure the data is written immediately
                }

                Console.WriteLine("User deleted successfully.");
                Console.WriteLine("Press any key to return to the User Manager Menu.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("User not found. Please try again.");
                Console.WriteLine("Press any key to return to the User Manager Menu.");
                Console.Clear();
                DeleteUser();
            }
        }

        public void UserMenu()
        {
            Program.MenuHeader();
            NotificationManager.NotificationHeader();
            Console.WriteLine("User Manager\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[1.]  Show All Users");
            Console.WriteLine("[2.]  Add User to System");
            Console.WriteLine("[3.]  Delete User from System");
            Console.WriteLine("[4.]  Return to Main Menu");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    ShowUsers();

                    Console.WriteLine("Press any key to return to the User Manager Menu.");
                    Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    break;
                case 2:
                    Console.Clear();
                    AddUser();
                    Console.Clear();
                    UserMenu();
                    break;
                case 3:
                    Console.Clear();
                    DeleteUser();
                    Console.Clear();
                    UserMenu();
                    break;
                case 4:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to return to Inventory Manager Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    UserMenu();
                    break;
            }
        }
    }
}