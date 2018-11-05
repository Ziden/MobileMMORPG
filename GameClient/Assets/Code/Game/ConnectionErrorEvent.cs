using Client.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Game
{
    class ConnectionErrorEvent : ClientEvent
    {
        public ClientTcpHandler ClientTcp;
    }
}
