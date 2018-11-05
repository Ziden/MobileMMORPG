using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.ConsoleCmds
{
    public abstract class ConsoleCommand
    {
        public abstract string GetCommand();

        public abstract bool RunCommand(string[] args);
    }
}
