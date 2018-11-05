using Storage.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    public static class Config
    {
        public static Player PLAYER_TEMPLATE = new Player()
        {
            speed = 10
        };
    }
}
