using Assets.Code.Net.PacketListeners;
using Client.Net.PacketListeners;
using Common;
using Common.Networking.Packets;
using UnityEngine;

namespace Assets.Code.Game
{
    public class ClientEvents
    {
        static ClientEvents()
        {
            new ClientEvents();
        }

        private static EventBus<BasePacket> eventHandler = new EventBus<BasePacket>();

        public static void Call(BasePacket e)
        {
            eventHandler.RunCallbacks(e);
        }

        public ClientEvents()
        {
            Debug.Log("REGISTERING EVENTS");
            eventHandler.RegisterListener(new PlayerListener());
            eventHandler.RegisterListener(new AccountListener());
            eventHandler.RegisterListener(new AssetListener());
            eventHandler.RegisterListener(new MapListener());
        }
    }

    public class ClientEvent
    {

    }
}
