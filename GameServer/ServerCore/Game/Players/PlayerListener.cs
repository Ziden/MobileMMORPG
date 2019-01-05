using CommonCode.EventBus;
using ServerCore.GameServer.Players.Evs;
using ServerCore.Networking.NetworkEvents;
using System;
using Common.Networking.Packets;
using ServerCore.GameServer.Entities;
using ServerCore.Game.Entities;

namespace ServerCore.GameServer.Players
{
    public class PlayerListener : IEventListener
    {
        [EventMethod]
        public void OnPlayerLogin(PlayerLoggedInEvent ev)
        {
            Log.Info($"Player {ev.Player.Login} Logged In with session {ev.Player.SessionId}", ConsoleColor.Yellow);

            var player = new OnlinePlayer();
            player.FromStored(ev.Player);
            player.Tcp = ev.Client;
            Server.Players.Add(player);
            ev.Client.OnlinePlayer = player;
        }

        [EventMethod]
        public void OnPlayerQuit(PlayerQuitEvent ev)
        {
            if (!Server.Players.Contains(ev.Player))
                return;

            if (ev.Player != null)
                Log.Info($"Player {ev.Player.Name} Disconnected", ConsoleColor.Yellow);
            else
                Log.Info($"Connection {ev.Client.ConnectionId} Disconnected", ConsoleColor.Yellow);

            if (ev.Player != null) // will be null if havent downloaded assets, need to fix this
            {
                ev.Player.Target = null;
                foreach(var targetingPlayer in ev.Player.BeingTargetedBy)
                {
                    targetingPlayer.Target = null;
                }
                ev.Player.BeingTargetedBy.Clear();
                Server.Map.UpdateEntityPosition(ev.Player, ev.Player.Position, null);
                Server.Players.Remove(ev.Player);
            }
            ev.Player = null;

            try
            {
                ev.Client.Stop();
            }
            catch (Exception e)
            {
                Log.Error("ERROR FINISHING SOCKET");
            }
        }

        [EventMethod]
        public void OnPlayerJoinEvent(PlayerJoinEvent ev)
        {
            var chunk = ev.Player.GetChunk();
            var player = ev.Player;
            var nearPlayers = player.GetPlayersNear();
            var packet = player.ToPacket();
           
            foreach (var nearPlayer in nearPlayers)
            {
                Log.Debug("WOLOLO");
                nearPlayer.Tcp.Send(packet);
                var otherPlayerPacket = nearPlayer.ToPacket();
                player.Tcp.Send(otherPlayerPacket);
            }
        }
    }
}
