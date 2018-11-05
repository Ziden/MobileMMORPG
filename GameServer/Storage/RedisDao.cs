using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Storage
{
    public class RedisDao<Entity>
    {
        public static string GetKeyNameFromInstance(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var keyAttr = prop.GetCustomAttribute<RedisKey>();
                if( keyAttr != null)
                {
                    var key = prop.GetValue(obj);
                    var stringKey = Convert.ChangeType(key, typeof(string));
                    return $"{keyAttr.Prefix}:{stringKey}";
                }
                
            }
            throw new Exception($"Could not get key for class {obj.GetType().Name}");
        }

        public static string GetKeyNameFromType(string keyValue)
        {
            var type = typeof(Entity);
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var keyAttr = prop.GetCustomAttribute<RedisKey>();
                return $"{keyAttr.Prefix}:{keyValue}";
            }
            throw new Exception($"Could not get key for class {type.Name}");
        }
    }
}
