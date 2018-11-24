using StackExchange.Redis;
using System.Linq;

namespace Storage
{
    public class Redis
    {
        public static IDatabase Db { get; set; }
        public static IServer Server { get; set; }

        public void Start()
        {
            ConfigurationOptions ops = new ConfigurationOptions()
            {
                AllowAdmin = true,
                KeepAlive = 1,
                EndPoints =
                    {
                        { "localhost", 6379 },
                    },
            };
            var con = ConnectionMultiplexer.Connect(ops);
            Db = con.GetDatabase();
            Server = con.GetEndPoints()
             .Select(endpoint =>
             {
                 var server = con.GetServer(endpoint);
                 return server;
             }).First();
        }
    }

}
