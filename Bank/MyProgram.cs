using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class MyProgram
    {
        public void Run()
        {
            List<User> users = new List<User>(); // List to store all users that are created
            User currentUser = null; // Variable to store the currently logged-in user

            // LOGIN / CREATE USER
            bool running = true; // Main menu loop control
            bool userLoggingIn = true; // Login loop control
            while (true)// Infinite loop to allow multiple logins and user creations
            {
                while (userLoggingIn) //Login loop for user creation and login
                {
                    Console.WriteLine("Login(1) or create user(2):");
                    string userInput = Console.ReadLine(); //Saving user input for login or create user in userInput

                    if (userInput == "1") // Login
                    {
                        Console.WriteLine("Enter username:");
                        string inputUsername = Console.ReadLine(); //Saving input username in inputUsername
                        Console.WriteLine("Enter password:");
                        string inputPassword = Console.ReadLine(); //Saving input password in inputPassword

                        User foundUser = users.Find(u => u.username == inputUsername && u.password == inputPassword); // Finding user with matching username and password

                        if (foundUser != null) // If user is found, log them in
                        {
                            currentUser = foundUser;
                            Console.WriteLine("Login successful!");
                            PressAnyKeyToContinue();
                            userLoggingIn = false; // exit login loop
                            running = true;    // enter main menu loop
                        }
                        else // Invalid login
                        {
                            Console.WriteLine("Invalid username or password.");
                            PressAnyKeyToContinue();
                        }
                    }
                    else if (userInput == "2") // Create user
                    {
                        Console.WriteLine("Create a username:");
                        string newUsername = Console.ReadLine(); //Saving new username in newUsername
                        Console.WriteLine("Create a password:");
                        string newPassword = Console.ReadLine(); //Saving new password in newPassword

                        if (users.Exists(u => u.username == newUsername)) // Check if username already exists 
                        {
                            Console.WriteLine("Username already exists. Try a different one."); // Error message for existing username
                            PressAnyKeyToContinue();
                            continue;
                        }

                        User newUser = new User(newUsername, newPassword); // Creating new user
                        users.Add(newUser); // Adding new user to the list of users
                        Console.WriteLine($"User {newUsername} created successfully!"); // Confirmation message for user creation
                        PressAnyKeyToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                        PressAnyKeyToContinue();
                    }
                }

                // MAIN MENU
                
                while (running)
                {
                    Console.WriteLine($"Welcome, {currentUser.username}!"); // Greeting the logged-in user
                    Console.WriteLine("1. Open new account");
                    Console.WriteLine("2. Deposit money into an account");
                    Console.WriteLine("3. View existing accounts and balances");
                    Console.WriteLine("4. View transaction history for an account");
                    Console.WriteLine("5. Send money from your account to another user");
                    Console.WriteLine("6. Log out");
                    Console.WriteLine("7. Exit");

                    string input = Console.ReadLine();
                    Console.Clear();

                    switch (input)
                    {
                        case "1": // Open new account
                            Console.WriteLine("Enter the name of the new account:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Enter starting balance:");
                            if (!float.TryParse(Console.ReadLine(), out float amount)) // Parsing starting balance input into float
                            {
                                Console.WriteLine("Invalid amount.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            Account newAccount = new Account(name, amount); // Creating new account
                            currentUser.accountsOnThisUser.Add(newAccount); // Adding account to user's account list

                            Console.WriteLine($"Account '{name}' created successfully with balance {amount}!"); // Confirmation message
                            PressAnyKeyToContinue();
                            break;
                        case "2": // Deposit
                            Console.WriteLine("Which of your accounts would you like to deposit into?");
                            string depositName = Console.ReadLine();

                            Account foundAccount = currentUser.accountsOnThisUser.Find(acc => acc.name == depositName); // getting account to deposit into and saving it in foundAccount
                            if (foundAccount != null) //deposit if account is found
                            {
                                Console.WriteLine("How much would you like to deposit?");
                                if (!float.TryParse(Console.ReadLine(), out float depositAmount)) // Parsing deposit amount input into float and saving it in depositAmount, then checking if it's valid
                                {
                                    Console.WriteLine("Invalid amount.");
                                    PressAnyKeyToContinue();
                                    break;
                                }

                                foundAccount.deposit(depositAmount);
                                Console.WriteLine($"Successfully deposited {depositAmount} into '{foundAccount.name}'. New balance: {foundAccount.balance}"); // Confirmation message for successful deposit
                            }
                            else
                            {
                                Console.WriteLine("Account not found.");
                            }
                            PressAnyKeyToContinue();
                            break;
                        case "3": // View accounts
                            Console.WriteLine("Your accounts and balances:");
                            foreach (Account account in currentUser.accountsOnThisUser) // Iterating through user's accounts and printing name and balance
                            {
                                Console.WriteLine($"{account.name}: {account.balance}"); // Printing account name and balance
                            }
                            PressAnyKeyToContinue();
                            break;
                        case "4": // Transaction history
                            Console.WriteLine("Which account's transaction history would you like to see?");
                            string historyName = Console.ReadLine(); //Saving account name for transaction history in historyName

                            Account historyAccount = currentUser.accountsOnThisUser.Find(acc => acc.name == historyName); // getting account for transaction history and saving it in historyAccount
                            if (historyAccount != null) // If account is found, print transaction history
                            {
                                Console.WriteLine($"Transaction history for '{historyName}':"); // Header for transaction history
                                foreach (string transaction in historyAccount.transactionHistory) // Iterating through transaction history
                                {
                                    Console.WriteLine(transaction); // Printing each transaction
                                }
                            }
                            else
                            {
                                Console.WriteLine("Account not found.");
                            }
                            PressAnyKeyToContinue();
                            break;
                        case "5": // Send money to another user's account
                            Console.WriteLine("Enter the name of your account to send money from:");
                            string fromName = Console.ReadLine(); //Saving sender account name in fromName
                            Account fromAccount = currentUser.accountsOnThisUser.Find(acc => acc.name == fromName); // getting sender's account and saving it in fromAccount

                            if (fromAccount == null) // Checks if sender account exists
                            {
                                Console.WriteLine("Sender account not found.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            Console.WriteLine("Enter the recipient's account name:");
                            string toName = Console.ReadLine(); //Saving recipient account name

                            // Search all users for the recipient account
                            Account toAccount = null;
                            User recipientUser = null;
                            foreach (User u in users) // Iterating through all users in the list called users
                            {
                                toAccount = u.accountsOnThisUser.Find(acc => acc.name == toName); // getting recipient's account and saving it in toAccount
                                if (toAccount != null) // If found, save the user and break
                                {
                                    recipientUser = u;
                                    break;
                                }
                            }

                            if (toAccount == null) // Checks if recipient account exists
                            {
                                Console.WriteLine("Recipient account not found.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            Console.WriteLine("Enter amount to send:");
                            if (!float.TryParse(Console.ReadLine(), out float sendAmount)) // Parses amount input into float
                            {
                                Console.WriteLine("Invalid amount.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            if (fromAccount.balance >= sendAmount) // Checks if sender has enough balance
                            {
                                fromAccount.balance -= sendAmount; // Deduct from sender
                                toAccount.balance += sendAmount; // Add to recipient

                                fromAccount.transactionHistory.Add($"Sent {sendAmount} to {toName}. New balance: {fromAccount.balance}"); // Log transaction for sender in their history
                                toAccount.transactionHistory.Add($"Received {sendAmount} from {fromName}. New balance: {toAccount.balance}"); // Log transaction for recipient in their history

                                Console.WriteLine($"Successfully sent {sendAmount} from '{fromName}' to '{toName}'."); // Confirmation message
                            }
                            else
                            {
                                Console.WriteLine("Insufficient funds.");
                            }
                            PressAnyKeyToContinue();
                            break;
                        case "6": // Log out
                            Console.WriteLine("Logging out...");
                            PressAnyKeyToContinue();

                            currentUser = null;   // Clear the logged-in user
                            userLoggingIn = true; // Re-enter login loop
                            running = false; // Exit main menu loop
                            break;
                        case "7": // Exit
                            Console.WriteLine("Thank you for using Nilas Bank. Goodbye!");
                            running = false;
                            return;
                        default:
                            Console.WriteLine("Invalid input. Try again.");
                            PressAnyKeyToContinue();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// waits for user to press any key before continuing
        /// </summary>
        void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
