using Storage.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    public static class Config
    {
        public static StoredPlayer PLAYER_TEMPLATE = new StoredPlayer()
        {
            MoveSpeed = 10
        };
    }
}
