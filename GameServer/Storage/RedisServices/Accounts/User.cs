using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.RedisServices.Accounts
{
    public class User
    {
        public string uuid { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}
