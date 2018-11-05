using StackExchange.Redis;
using Storage.Login;
using Storage.Players;
using System;

namespace Storage.TestDataBuilder
{
    public class TestDb
    {
        public static void Create()
        {
            Console.WriteLine("Creating admin account");
            try
            {
                var testPlayerTemplate = new Player()
                {
                    speed = 10
                };

                Redis.Server.FlushAllDatabases();
                AccountService.RegisterAccount("admin", "wololo", "admin@gmail.com", testPlayerTemplate);
            } catch(AccountError err)
            {
                Console.WriteLine("Admin account was already created");
            }
            Console.WriteLine("Admin account ready");
        }
    }
}
