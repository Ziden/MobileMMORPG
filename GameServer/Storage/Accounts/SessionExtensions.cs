using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Login
{
    public static class SessionExtensions
    {
        public static bool ValidSession(this Session session)
        {
            var key = RedisHash<Session>.GetKeyNameFromInstance(session);
            return Redis.Db.KeyExists(key);
        }
    }
}
