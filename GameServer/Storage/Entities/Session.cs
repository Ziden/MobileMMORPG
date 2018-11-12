using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Login
{
    public class Session : RedisEntity
    {
        [RedisField("sid")]
        [RedisKey("ss")]
        public string SessionUid { get; set; }

        [RedisField("uid")]
        public string PlayerUid { get; set; }

        [RedisField("e")]
        public long Expires { get; set; }
    }
}
