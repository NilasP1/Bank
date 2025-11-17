using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class User
    {
        public string username; // username of the user
        public string password; // password of the user

        public List<Account> accountsOnThisUser = new List<Account>(); // list of accounts associated with this user

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified username and password.
        /// </summary>
        /// <remarks>Ensure that the provided username and password meet any application-specific
        /// validation requirements before creating a new instance of the <see cref="User"/> class.</remarks>
        /// <param name="usercreatename">The username to associate with the user. Cannot be null or empty.</param>
        /// <param name="passwordcreate">The password to associate with the user. Cannot be null or empty.</param>
        public User(string usercreatename, string passwordcreate)
        {
            this.username = usercreatename;
            this.password = passwordcreate;
        }
    }
}
