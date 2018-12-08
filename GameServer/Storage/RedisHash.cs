using StackExchange.Redis;
using System.Collections.Generic;

namespace Storage
{
    public class RedisHash<Type> : RedisDao<Type> where Type : RedisEntity, new()
    {
        public static void Set(Type t)
        {
            var hash = DataSerializer.ToRedisHash(t);
            var key = GetKeyNameFromInstance(t);
            Redis.Db.HashSet(key, hash);
        }

        public static void Update(string uuid, string [] fieldNames, object [] values)
        {
            var key = GetKeyNameFromType(uuid);
            var hashEntries = new List<HashEntry>();

            for(var i = 0; i < fieldNames.Length; i++)
            {
                var fieldName = fieldNames[i];
                var value = values[i];
                var hashEntry = DataSerializer.GetHashEntry<Type>(fieldName, value);
                hashEntries.Add(hashEntry.Value);
            }

            Redis.Db.HashSet(key, hashEntries.ToArray());
        }

        public static Type Get(string keyValue)
        {
            var key = GetKeyNameFromType(keyValue);
            var hash = Redis.Db.HashGetAll(key);
            return DataSerializer.FromRedisHash<Type>(hash);
        }
    }
}
