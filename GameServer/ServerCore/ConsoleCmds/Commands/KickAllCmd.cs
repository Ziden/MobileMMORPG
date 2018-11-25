using ServerCore.GameServer.Players.Evs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.ConsoleCmds.Commands
{
    public class KickAllCmd : ConsoleCommand
    {
        public override string GetCommand()
        {
            return "kickall";
        }

        public override bool RunCommand(string[] args)
        {
            var players = Server.Players.ToArray();
            int ct = 0;
            foreach(var player in players)
            {
                Server.Events.Call(new PlayerQuitEvent()
                {
                    Client = player.Tcp,
                    Player = player,
                    Reason = QuitReason.KICKED
                });
                ct++;
            }

            return true;
        }
    }
}
