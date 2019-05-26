using Storage.Login;
using Storage.Players;
using System;

namespace Storage.TestDataBuilder
{

    /*
     * LISTA TODO:
     * 
     * - FAZER O JOGADOR MORRER
     * - ARRUMAR OS PIXELS CAGADOS DOS SPRITES
     * 
     */
    public class TestDb
    {
        public static StoredPlayer TEST_PLAYER = new StoredPlayer()
        {
            X = 0,
            Y = 0,
            SpawnX = 0,
            SpawnY = 0,

            MoveSpeed = 60,
            BodySpriteIndex = 2,
            HeadSpriteIndex = 2,
            ChestSpriteIndex = 2,
            LegsSpriteIndex = 2,

            Atk = 4,
            Def = 1,
            HP = 10,
            MaxHp = 10,
            AtkSpeed = 5
        };

        public static void Create()
        {
            Console.WriteLine("Creating admin account");
            try
            {
                Redis.Server.FlushAllDatabases();
                AccountService.RegisterAccount("a", "a", "a@gmail.com", TEST_PLAYER);
                AccountService.RegisterAccount("b", "b", "b@gmail.com", TEST_PLAYER);
            } catch(AccountError err)
            {
                Console.WriteLine("Admin account was already created");
            }
            Console.WriteLine("Admin account ready");
        }
    }
}
