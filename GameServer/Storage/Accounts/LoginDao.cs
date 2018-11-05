using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Login
{
    public static class LoginDao
    {
        public static string GetUserId(string login)
        {
            return Redis.Db.StringGet($"login:{login}");
        }

        public static bool LoginExists(string login)
        {
            return Redis.Db.KeyExists($"login:{login}");
        }

        public static void SetUserId(string login, string userid)
        {
            Redis.Db.StringSet($"login:{login}", userid);
        }
    }
}
