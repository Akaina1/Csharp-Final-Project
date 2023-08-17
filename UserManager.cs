using CsvHelper;
using System.Globalization;

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
                    if (usernameExists == true) {break;}

                    // if username doesn't exist, tell user to retype username
                    Console.Clear();
                    Console.WriteLine("Username does not exist. Please try again.");
                    Console.ReadLine();
                    Console.Clear();
                    LoginScreen("", "");                         
                }
                
                Console.Clear();
                LoginScreen(username,"");
                bool passwordMatches = false;

                while (passwordMatches == false)
                {
                    // get password from user
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();

                    // check if password matches username
                    passwordMatches = records.Any(x => x.Password == password);

                    //if pasword matches, break
                    if (passwordMatches == true) { break;}

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
    }
}