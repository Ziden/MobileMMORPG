using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using ServerCore.GameServer.Players.Evs;
using ServerCore.Networking.NetworkEvents;
using System;
using AutoMapper;
using Common.Networking.Packets;
using MapHandler;

namespace ServerCore.GameServer.Players
{
    public class PlayerListener : IEventListener
    {
        [EventMethod]
        public void OnPlayerLogin(PlayerLoggedInEvent ev)
        {
            Log.Info($"Player {ev.Player.Login} Logged In with session {ev.Player.SessionId}", ConsoleColor.Yellow);
            var player = Mapper.Map<OnlinePlayer>(ev.Player);
            player.Tcp = ev.Client;
            Server.Players.Add(player);
            ev.Client.OnlinePlayer = player;

            // Going to start sending asset validations
            ev.Client.Send(new AssetsReadyPacket());

            // check if the player already have the tilesets
            foreach (var tileset in Server.Map.Tilesets)
            {
                ev.Client.Send(new AssetPacket()
                {
                    ResquestedImageName = tileset.Key,
                    AssetType = AssetType.TILESET
                });
            }

            // check if the player have the main sprites
            ev.Client.Send(new AssetPacket()
            {
                ResquestedImageName = "sprites.png",
                AssetType = AssetType.SPRITE
            });

            ev.Client.Send(new AssetPacket()
            {
                ResquestedImageName = "bodies.png",
                AssetType = AssetType.SPRITE
            });

            ev.Client.Send(new AssetPacket()
            {
                ResquestedImageName = "legs.png",
                AssetType = AssetType.SPRITE
            });

            ev.Client.Send(new AssetPacket()
            {
                ResquestedImageName = "heads.png",
                AssetType = AssetType.SPRITE
            });

            ev.Client.Send(new AssetPacket()
            {
                ResquestedImageName = "chests.png",
                AssetType = AssetType.SPRITE
            });

            // end of assets validation
            ev.Client.Send(new AssetsReadyPacket());
        }

        [EventMethod]
        public void OnPlayerQuit(PlayerQuitEvent ev)
        {
            if (ev.Player != null)
                Log.Info($"Player {ev.Player.Login} Disconnected", ConsoleColor.Yellow);
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
            var packet = new PlayerPacket()
            {
                Name = player.Login,
                X = player.X,
                Y = player.Y,
                UserId = player.UserId,
                Speed = player.speed,
                SpriteIndex = player.SpriteIndex
            };
           
            foreach (var nearPlayer in nearPlayers)
            {
                Log.Info("SENDING TO " + nearPlayer.X);
                nearPlayer.Tcp.Send(packet);

                var otherPlayerPacket = new PlayerPacket()
                {
                    Name = nearPlayer.Login,
                    X = nearPlayer.X,
                    Y = nearPlayer.Y,
                    UserId = nearPlayer.UserId,
                    Speed = nearPlayer.speed,
                    SpriteIndex = nearPlayer.SpriteIndex
                };
                player.Tcp.Send(otherPlayerPacket);
            }
        }

        [EventMethod]
        public void OnPlayerMove(PlayerMoveEvent ev)
        {
            var fromChunkX = ev.From.X >> 4;
            var fromChunkY = ev.From.Y >> 4;

            var toChunkX = ev.From.X >> 4;
            var toChunkY = ev.From.Y >> 4;

            var toChunk = Server.Map.GetChunk(toChunkX, toChunkY);

            var nearPlayers = ev.Player.GetPlayersNear();
            var movePacket = new PlayerMovePacket()
            {
                From = ev.From,
                To = ev.To,
                UserId = ev.Player.UserId
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
