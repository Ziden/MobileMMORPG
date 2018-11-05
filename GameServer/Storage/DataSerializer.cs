using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Storage
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RedisField : Attribute
    {
        public string Name;

        public RedisField(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RedisKey : Attribute
    {
        public string Prefix;

        public RedisKey(string prefix)
        {
            this.Prefix = prefix;
        }
    }

    public static class DataSerializer
    {

        private static PropertyInfo GetProperty(object obj, string name)
        {
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.Name == name)
                    return prop;
                var propertyRedisAttribute = prop.GetCustomAttribute<RedisField>();
                if (propertyRedisAttribute.Name == name)
                    return prop;

            }
            return null;
        }

        public static HashEntry? GetHashEntry<T>(string name, object value)
        {
            if (value is double)
            {
                return new HashEntry(name, (double)value);
            }
            else if (value is int)
            {
                return new HashEntry(name, (int)value);
            }
            else if (value is string)
            {
                return new HashEntry(name, (string)value);
            }
            return null;
        }

        public static T FromRedisHash<T>(HashEntry[] entries) where T : RedisEntity, new()
        {
            T thing = new T();
            foreach (var entry in entries)
            {
                var property = GetProperty(thing, entry.Name);
                if (property != null)
                {
                    var convertedValue = Convert.ChangeType(entry.Value, property.PropertyType);
                    property.SetValue(thing, convertedValue);
                }
            }
            return thing;
        }

        // Transform an object into a redis hash
        public static HashEntry[] ToRedisHash<T>(T objectInstance) where T : RedisEntity
        {
            List<HashEntry> converted = new List<HashEntry>();

            var type = objectInstance.GetType();

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var name = property.Name;
                var propertyRedisAttribute = property.GetCustomAttribute<RedisField>();
                if (propertyRedisAttribute != null)
                {
                    name = propertyRedisAttribute.Name;
                }
                var value = property.GetValue(objectInstance);

                var HashEntry = GetHashEntry<T>(name, value);
                if (HashEntry.HasValue)
                {
                    converted.Add(HashEntry.Value);
                }
            }
            return converted.ToArray();
        }
    }
}
