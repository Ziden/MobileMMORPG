using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Login
{
    public static class SessionExtensions
    {
        public static bool ValidSession(string sessionId)
        {
           // var key = RedisHash<Session>.GetKeyNameFromType(sessionId);
           // var haveSession = Redis.Db.KeyExists(key);
           // if (!haveSession)
           //     return false;
            var session = RedisHash<Session>.Get(sessionId);
            if (session == null)
                return false;
            return true;
        }
    }
}
