using Common;
using Common.Networking.Packets;
using ServerCore.Utils.Scheduler;
using System;

// Main game thread, will process things in sync, reading incoming packets from the packet queue
// processing them, calculating world changes, sending packets back etc

namespace ServerCore
{
    public class GameThread : BaseThread
    {
        public bool Running { get; set; }

        private long LastTick;

        public GameThread()
        {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }

        public override void RunThread()
        {
            Log.Info("Starting Game Thread");
            LastTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (Running)
            {
                var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if(now - LastTick > 50)
                {
                    Log.Error("Took more then 50ms to do a Game Loop");
                }
                ProcessPlayerInput();
                ProcessEntities();
                GameScheduler.RunTasks(now);
                LastTick = now;
            }
        }

        public void ProcessPlayerInput()
        {
            while(Server.PacketsToProccess.Count > 0)
            {
                BasePacket recievedPacket = null;
                Server.PacketsToProccess.TryDequeue(out recievedPacket);
                if (recievedPacket != null)
                {
                    Server.Events.Call(recievedPacket);
                }
            }
        }

        public void ProcessEntities()
        {
           // foreach (var onlinePlayer in Server.Players.ToArray())
           //     continue;
                //ChunkProvider.CheckChunks(onlinePlayer);
        }


    }
}
