using Common;
using Common.Networking.Packets;
using Common.Scheduler;
using System;

// Main game thread, will process things in sync, reading incoming packets from the packet queue
// processing them, calculating world changes, sending packets back etc

namespace ServerCore
{
    public class GameThread : BaseThread
    {

        public static long TIME_MS_NOW = 0;

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
                TIME_MS_NOW = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if(TIME_MS_NOW - LastTick > 50)
                {
                    Log.Error("Took more then 50ms to do a Game Loop");
                }
                ProcessPackets();
                GameScheduler.RunTasks(TIME_MS_NOW);
                LastTick = TIME_MS_NOW;
            }
        }


        public void ProcessPackets()
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
    }
}
