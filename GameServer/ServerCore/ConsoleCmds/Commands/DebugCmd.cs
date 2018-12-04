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
                if(args[0]=="chunk")
                {
                    foreach(var chunk in Server.Map.Chunks.Values)
                    {
                        if(chunk.PlayersInChunk.Count > 0)
                        {
                            Log.Info($"Chunk {chunk.x} - {chunk.x} have players {chunk.PlayersInChunk.Count}");
                        }
                    }
                }

                // SPAWNERS
                if(args[0]=="spawners")
                {
                    Server.Map.LoadAllSpawners();
                }
            }

          
            return true;
        }
    }
}
