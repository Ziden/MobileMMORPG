
using Common;
using Common.Networking.Packets;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players;
using ServerCore.Networking;
using ServerCore.Networking.PacketListeners;
using ServerCore.PacketListeners;

namespace ServerCore
{
    public interface IEvent { }

    public class ServerEvents
    {

        private static EventBus<IEvent> _eventListener = new EventBus<IEvent>();

        private static EventBus<BasePacket> _packetListener = new EventBus<BasePacket>();

        public static void Call(IEvent e)
        {
            _eventListener.RunCallbacks(e);
        }

        public static void Call(BasePacket p)
        {
            _packetListener.RunCallbacks(p);
        }

        public ServerEvents()
        {
            // EVENT LISTENERS
            _eventListener.RegisterListener(new PlayerListener());
            _eventListener.RegisterListener(new MapListener());

            // PACKET LISTENERS
            _packetListener.RegisterListener(new LoginPacketListener());
            _packetListener.RegisterListener(new AssetListener());
            _packetListener.RegisterListener(new PlayerPacketListener());
        }
    }
}
