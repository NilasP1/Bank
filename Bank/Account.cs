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

        public List<string> transactionHistory = new List<string>();

        public Account(string name, float balance)
        {
            this.balance = balance;
            this.name = name;

            transactionHistory.Add($"Account created with starting balance: {balance}");
            
        }

        public Account()
        {
            balance = 0;
            name = "No name on the account";

            transactionHistory.Add($"Account created with starting balance: {balance}");

        }

        public void deposit(float amount)
        {
            balance += amount;
            transactionHistory.Add($"Deposited {amount}. New balance: {balance}");
        }
    }
}
