using Assets.Code.Game.PacketListeners;
using Assets.Code.Net.PacketListeners;
using Client.Net.PacketListeners;
using Common;
using Common.Networking.Packets;

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
            eventHandler.RegisterListener(new PlayerListener());
            eventHandler.RegisterListener(new AccountListener());
            eventHandler.RegisterListener(new AssetListener());
            eventHandler.RegisterListener(new MapListener());
            eventHandler.RegisterListener(new EntityListener());
        }
    }

    public class ClientEvent
    {

    }
}
