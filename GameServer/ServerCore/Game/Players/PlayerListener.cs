using CommonCode.EventBus;
using ServerCore.GameServer.Players.Evs;
using ServerCore.Networking.NetworkEvents;
using System;
using Common.Networking.Packets;

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
            if (ev.Player != null)
                Log.Info($"Player {ev.Player.Name} Disconnected", ConsoleColor.Yellow);
            else
                Log.Info($"Connection {ev.Client.ConnectionId} Disconnected", ConsoleColor.Yellow);

            try
            {
                if (ev.Player != null)
                {
                    var chunk = ev.Player.GetChunk();
                    chunk.PlayersInChunk.Remove(ev.Player);
                    Server.Players.Remove(ev.Player);
                }

                ev.Player = null;
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
            chunk.PlayersInChunk.Add(ev.Player);
            var player = ev.Player;
            var nearPlayers = player.GetPlayersNear();
            var packet = player.ToPacket();
           
            foreach (var nearPlayer in nearPlayers)
            {
                nearPlayer.Tcp.Send(packet);
                var otherPlayerPacket = nearPlayer.ToPacket();
                player.Tcp.Send(otherPlayerPacket);
            }
        }

        [EventMethod]
        public void OnPlayerMove(PlayerMoveEvent ev)
        {
            var fromChunkX = ev.From.X >> 4;
            var fromChunkY = ev.From.Y >> 4;

            var toChunkX = ev.To.X >> 4;
            var toChunkY = ev.To.Y >> 4;

            var toChunk = Server.Map.GetChunk(toChunkX, toChunkY);

            var nearPlayers = ev.Player.GetPlayersNear();
            var movePacket = new EntityMovePacket()
            {
                From = ev.From,
                To = ev.To,
                UID = ev.Player.UID
            };

            foreach (var nearPlayer in nearPlayers)
            {
                nearPlayer.Tcp.Send(movePacket);
            }

            // Changed chunk
            if (fromChunkX != toChunkX || fromChunkY != toChunkY)
            {
                var fromChunk = Server.Map.GetChunk(fromChunkX, fromChunkY);

                fromChunk.PlayersInChunk.Remove(ev.Player);
                toChunk.PlayersInChunk.Add(ev.Player);
            }
        }
    }
}
