using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.ConsoleCmds.Commands
{
    public class OnlineCmd : ConsoleCommand
    {
        public override string GetCommand()
        {
            return "online";
        }

        public override bool RunCommand(string[] args)
        {
            var players = Server.Players.ToArray();
            var result = "";
            foreach(var player in players)
            {
                result += $"{player.Login} ";
            }
            Log.Info($"Online Players: {players.Count}");
            Log.Info(result);
            return true;
        }
    }
}
