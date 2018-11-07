using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using ServerCore.GameServer.Players.Evs;
using ServerCore.Networking.NetworkEvents;
using System;
using AutoMapper;
using Common.Networking.Packets;

namespace ServerCore.GameServer.Players
{
    public class PlayerListener : IEventListener
    {
        [EventMethod]
        public void OnPlayerLogin(PlayerLoginEvent ev)
        {
            Log.Info($"Player {ev.Player.Login} Logged In with session {ev.Player.SessionId}", ConsoleColor.Yellow);
            var onlinePlayer = Mapper.Map<OnlinePlayer>(ev.Player);
            onlinePlayer.Tcp = ev.Client;
            Server.Players.Add(onlinePlayer);
            ev.Client.OnlinePlayer = onlinePlayer;

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
            if(ev.Player != null)
                Log.Info($"Player {ev.Player.Login} Disconnected", ConsoleColor.Yellow);
            else
                Log.Info($"Connection {ev.Client.ConnectionId} Disconnected", ConsoleColor.Yellow);

            try
            {
                if(ev.Player != null)
                    Server.Players.Remove(ev.Player);
                ev.Client.Stop();
            } catch(Exception e)
            {
                Log.Error("ERROR FINISHING SOCKET");
            }
        }
    }
}
