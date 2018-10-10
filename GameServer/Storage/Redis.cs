using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage
{
    public class Redis
    {
        public static IDatabase db { get; set; }

        public void Start()
        {
            db = ConnectionMultiplexer.Connect("localhost").GetDatabase();
        }
    }

}
