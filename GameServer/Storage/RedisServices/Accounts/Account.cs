using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.RedisServices.Accounts
{
    public class Account
    {
        public string login { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}
