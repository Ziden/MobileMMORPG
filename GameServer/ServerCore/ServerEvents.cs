
using Common;
using Common.Networking.Packets;
using ServerCore.Game.Entities;
using ServerCore.Game.GameMap;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Players;
using ServerCore.Networking;
using ServerCore.Networking.PacketListeners;
using ServerCore.PacketListeners;

namespace ServerCore
{
    public interface IEvent { }

    public class ServerEvents
    {
        private EventBus<IEvent> _eventListener = new EventBus<IEvent>();

        private EventBus<BasePacket> _packetListener = new EventBus<BasePacket>();

        public void Clear()
        {
            _packetListener = new EventBus<BasePacket>();
            _eventListener = new EventBus<IEvent>();
        }

        public void Call(IEvent e)
        {
            _eventListener.RunCallbacks(e);
        }

        public void Call(BasePacket p)
        {
            _packetListener.RunCallbacks(p);
        }

        public ServerEvents()
        {
            // EVENT LISTENERS
            _eventListener.RegisterListener(new PlayerListener());
            _eventListener.RegisterListener(new MapListener());
            _eventListener.RegisterListener(new MonsterListener());
            _eventListener.RegisterListener(new EntityListener());

            // PACKET LISTENERS
            _packetListener.RegisterListener(new LoginPacketListener());
            _packetListener.RegisterListener(new AssetPacketListener());
            _packetListener.RegisterListener(new PlayerPacketListener());
        }
    }
}
