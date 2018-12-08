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
                var testPlayerTemplate = new StoredPlayer()
                {
                    MoveSpeed = 10,
                    BodySpriteIndex = 2,
                    HeadSpriteIndex = 2,
                    ChestSpriteIndex = 2,
                    LegsSpriteIndex = 2
                };

                Redis.Server.FlushAllDatabases();
                AccountService.RegisterAccount("a", "a", "a@gmail.com", testPlayerTemplate);
                AccountService.RegisterAccount("b", "b", "b@gmail.com", testPlayerTemplate);
            } catch(AccountError err)
            {
                Console.WriteLine("Admin account was already created");
            }
            Console.WriteLine("Admin account ready");
        }
    }
}
