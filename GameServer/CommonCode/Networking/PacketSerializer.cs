using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common.Networking
{
    public static class PacketSerializer
    {
        public static byte [] Serialize(object anySerializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, anySerializableObject);
                return memoryStream.ToArray();
            }
        }

        public static object Deserialize(byte []  message)
        {
            using (var memoryStream = new MemoryStream(message))
                return (new BinaryFormatter()).Deserialize(memoryStream);
        }
    }
}
