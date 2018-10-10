using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage
{
    public abstract class RedisService
    {

        public static IDatabase db { get; set; }

        public void Start()
        {
            db = ConnectionMultiplexer.Connect("localhost").GetDatabase();
        }

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };

        protected string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        protected object Deserialize(string data)
        {
            return JsonConvert.DeserializeObject(data, _settings);
        }

    }
}
