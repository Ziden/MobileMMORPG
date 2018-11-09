using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCore.ConsoleCmds.Commands
{
    public class InfoCmd : ConsoleCommand
    {
        public override string GetCommand()
        {
            return "info";
        }

        public override bool RunCommand(string[] args)
        {
            if(args.Length == 0)
            {
                Log.Info("Use info <playername>");
                return false;
            }
            var playerName = args[0];
            var player = Server.Players.ToArray().FirstOrDefault(p => p.Login.ToLower() == playerName.ToLower());
            if(player == null)
            {
                Log.Error($"Player {playerName} not found");
                return false;
            }
            Log.Info("| Login: " + playerName);
            Log.Info("| X: " + player.X);
            Log.Info("| Y: " + player.Y);
            Log.Info("| UserId: " + player.UserId);
            Log.Info("| ConnectionId: " + player.Tcp.ConnectionId);
            Log.Info("| Ping: " + player.Tcp.Latency+"ms");
            return true;
        }
    }
}
