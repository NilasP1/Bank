using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class Account
    {
        public float balance; 
        public string name;

        public List<string> transactionHistory = new List<string>(); // List to store transaction history

        /// <summary>
        /// constructor for account class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="balance"></param>
        public Account(string name, float balance) 
        {
            this.balance = balance;
            this.name = name;

            transactionHistory.Add($"Account created with starting balance: {balance}");
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class with a default balance of zero and a default
        /// name.
        /// </summary>
        /// <remarks>The account is initialized with a balance of 0 and the name "No name on the account".
        /// A transaction history entry is added to indicate the account creation.</remarks>
        public Account()
        {
            balance = 0;
            name = "No name on the account";

            transactionHistory.Add($"Account created with starting balance: {balance}");

        }

        /// <summary>
        /// Deposits the specified amount into the account.
        /// </summary>
        /// <remarks>Updates the account balance and records the transaction in the transaction
        /// history.</remarks>
        /// <param name="amount">The amount to deposit. Must be a positive value.</param>
        public void deposit(float amount)
        {
            balance += amount;
            transactionHistory.Add($"Deposited {amount}. New balance: {balance}");
        }
    }
}
