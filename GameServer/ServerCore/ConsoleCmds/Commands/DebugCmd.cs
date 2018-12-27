using System.Linq;

namespace ServerCore.ConsoleCmds.Commands
{
    public class DebugCmd : ConsoleCommand
    {
        public override string GetCommand()
        {
            return "debug";
        }

        public override bool RunCommand(string[] args)
        {
            if(args.Length == 1)
            {
                // CHUNK
                if(args[0]=="playerposition")
                {
                    var targetPlayer = Server.Players.First();
                    Log.Info("Running Consistency Check");

                }

                // SPAWNERS
                if(args[0]=="reloadspawners")
                {
                    Server.Map.LoadAllSpawners();
                }
            }

          
            return true;
        }
    }
}
