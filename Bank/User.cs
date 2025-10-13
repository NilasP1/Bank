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
        public string username;
        public string password;

        public List<Account> accountsOnThisUser = new List<Account>();

        public User(string usercreatename, string passwordcreate)
        {
            this.username = usercreatename;
            this.password = passwordcreate;
        }
    }
}
