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
            List<User> users = new List<User>();
            User currentUser = null;

            // --- LOGIN / CREATE USER ---
            bool running = true;
            bool userLoggingIn = true;
            while (true)
            {
                while (userLoggingIn)
                {
                    Console.WriteLine("Login(1) or create user(2):");
                    string userInput = Console.ReadLine();

                    if (userInput == "1") // Login
                    {
                        Console.WriteLine("Enter username:");
                        string inputUsername = Console.ReadLine();
                        Console.WriteLine("Enter password:");
                        string inputPassword = Console.ReadLine();

                        User foundUser = users.Find(u => u.username == inputUsername && u.password == inputPassword);

                        if (foundUser != null)
                        {
                            currentUser = foundUser;
                            Console.WriteLine("Login successful!");
                            PressAnyKeyToContinue();
                            userLoggingIn = false; // exit login loop
                            running = true;    // enter main menu loop
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                            PressAnyKeyToContinue();
                        }
                    }
                    else if (userInput == "2") // Create user
                    {
                        Console.WriteLine("Create a username:");
                        string newUsername = Console.ReadLine();
                        Console.WriteLine("Create a password:");
                        string newPassword = Console.ReadLine();

                        if (users.Exists(u => u.username == newUsername))
                        {
                            Console.WriteLine("Username already exists. Try a different one.");
                            PressAnyKeyToContinue();
                            continue;
                        }

                        User newUser = new User(newUsername, newPassword);
                        users.Add(newUser);
                        Console.WriteLine($"User {newUsername} created successfully!");
                        PressAnyKeyToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                        PressAnyKeyToContinue();
                    }
                }

                // --- MAIN MENU ---
                
                while (running)
                {
                    Console.WriteLine($"Welcome, {currentUser.username}!");
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
                            if (!float.TryParse(Console.ReadLine(), out float amount))
                            {
                                Console.WriteLine("Invalid amount.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            Account newAccount = new Account(name, amount);
                            currentUser.accountsOnThisUser.Add(newAccount);

                            Console.WriteLine($"Account '{name}' created successfully with balance {amount}!");
                            PressAnyKeyToContinue();
                            break;

                        case "2": // Deposit
                            Console.WriteLine("Which of your accounts would you like to deposit into?");
                            string depositName = Console.ReadLine();

                            Account foundAccount = currentUser.accountsOnThisUser.Find(acc => acc.name == depositName);
                            if (foundAccount != null)
                            {
                                Console.WriteLine("How much would you like to deposit?");
                                if (!float.TryParse(Console.ReadLine(), out float depositAmount))
                                {
                                    Console.WriteLine("Invalid amount.");
                                    PressAnyKeyToContinue();
                                    break;
                                }

                                foundAccount.deposit(depositAmount);
                                Console.WriteLine($"Successfully deposited {depositAmount} into '{foundAccount.name}'. New balance: {foundAccount.balance}");
                            }
                            else
                            {
                                Console.WriteLine("Account not found.");
                            }
                            PressAnyKeyToContinue();
                            break;

                        case "3": // View accounts
                            Console.WriteLine("Your accounts and balances:");
                            foreach (Account account in currentUser.accountsOnThisUser)
                            {
                                Console.WriteLine($"{account.name}: {account.balance}");
                            }
                            PressAnyKeyToContinue();
                            break;

                        case "4": // Transaction history
                            Console.WriteLine("Which account's transaction history would you like to see?");
                            string historyName = Console.ReadLine();

                            Account historyAccount = currentUser.accountsOnThisUser.Find(acc => acc.name == historyName);
                            if (historyAccount != null)
                            {
                                Console.WriteLine($"Transaction history for '{historyName}':");
                                foreach (string transaction in historyAccount.transactionHistory)
                                {
                                    Console.WriteLine(transaction);
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
                            string fromName = Console.ReadLine();
                            Account fromAccount = currentUser.accountsOnThisUser.Find(acc => acc.name == fromName);

                            if (fromAccount == null)
                            {
                                Console.WriteLine("Sender account not found.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            Console.WriteLine("Enter the recipient's account name:");
                            string toName = Console.ReadLine();

                            // Search all users for the recipient account
                            Account toAccount = null;
                            User recipientUser = null;
                            foreach (User u in users)
                            {
                                toAccount = u.accountsOnThisUser.Find(acc => acc.name == toName);
                                if (toAccount != null)
                                {
                                    recipientUser = u;
                                    break;
                                }
                            }

                            if (toAccount == null)
                            {
                                Console.WriteLine("Recipient account not found.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            Console.WriteLine("Enter amount to send:");
                            if (!float.TryParse(Console.ReadLine(), out float sendAmount))
                            {
                                Console.WriteLine("Invalid amount.");
                                PressAnyKeyToContinue();
                                break;
                            }

                            if (fromAccount.balance >= sendAmount)
                            {
                                fromAccount.balance -= sendAmount;
                                toAccount.balance += sendAmount;

                                fromAccount.transactionHistory.Add($"Sent {sendAmount} to {toName}. New balance: {fromAccount.balance}");
                                toAccount.transactionHistory.Add($"Received {sendAmount} from {fromName}. New balance: {toAccount.balance}");

                                Console.WriteLine($"Successfully sent {sendAmount} from '{fromName}' to '{toName}'.");
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
                            running = false;
                            break;
                        case "7": // Exit
                            Console.WriteLine("Thank you for using Nilas Bank. Goodbye!");
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid input. Try again.");
                            PressAnyKeyToContinue();
                            break;
                    }
                }
            }
        }

        void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
