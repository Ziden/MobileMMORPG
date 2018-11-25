using ServerCore.ConsoleCmds;
using ServerCore.ConsoleCmds.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCore.ConsoleCmds
{
    public class CommandHandler
    {
        public Dictionary<string, ConsoleCommand> _registeredCommands = new Dictionary<string, ConsoleCommand>();

        public CommandHandler()
        {
            RegisterCommand(new OnlineCmd());
            RegisterCommand(new KickAllCmd());
            RegisterCommand(new InfoCmd());
            RegisterCommand(new DebugCmd());
        }

        public List<string> RegisteredCommands()
        {
            var ks = _registeredCommands.Keys.ToList();
            return ks;
        }

        public void RegisterCommand(ConsoleCommand cmd)
        {
            _registeredCommands.Add(cmd.GetCommand().ToLower(), cmd);
        }

        public bool RunCommand(string cmd, string [] args)
        {
            if (!_registeredCommands.ContainsKey(cmd.ToLower()))
                return false;
            _registeredCommands[cmd].RunCommand(args);
            return true;
        }

      

    }
}
