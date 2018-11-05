using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class LoginPacket : BasePacket
    {
        public string Login;
        public string Password;
    }
}
