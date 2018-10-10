using Storage.RedisServices.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.TestDataBuilder
{
    public class TestDb
    {
        public static void Create()
        {

            Console.WriteLine("Creating admin account");

            try
            {
                new AccountService().RegisterAccount("admin", "wololo", "admin@gmail.com");
            } catch(AccountError err)
            {
                Console.WriteLine("Admin account was already created");
            }
            Console.WriteLine("Admin account ready");
        }
    }
}
