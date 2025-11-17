using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class SavingsAccount : Account
    {
        // Interest rate (5%)
        public float InterestRate { get; private set; }
        private Timer interestTimer;

        // Constructor
        public SavingsAccount(string name, float balance, float interestRate = 0.05f) : base(name, balance)
        {
            InterestRate = interestRate; // Default to 5% if not specified
            transactionHistory.Add($"Savings account created with interest rate: {InterestRate * 100}%"); //added interest rate to history

            StartInterestGrowth(); // Start automatic interest growth
        }

        public SavingsAccount() : base()
        {
            InterestRate = 0.05f; // Default to 5%
            transactionHistory.Add($"Savings account created with interest rate: {InterestRate * 100}%"); //added interest rate to history

            StartInterestGrowth(); // Start automatic interest growth
        }

        // Method to grow money by interest
        public void ApplyInterest() 
        {
            float interest = balance * InterestRate; // Calculate interest
            balance += interest; // Apply interest to balance
            transactionHistory.Add($"Interest applied: {interest}. New balance: {balance}"); // Log interest application
        }

        // Override withdrawal behavior: not allowed
        public void Withdraw(float amount)
        {
            Console.WriteLine("Withdrawals are not allowed from a savings account."); // Inform user
            transactionHistory.Add($"Attempted withdrawal of {amount} denied."); // Log denied withdrawal
        }

        // Start automatic interest growth
        private void StartInterestGrowth()
        {
            // Apply interest every 5 seconds (you can adjust interval)
            interestTimer = new Timer(_ =>
            {
                ApplyInterest();
            }, null, 0, 5000); // 5000 ms = 5 seconds
        }
    }
}