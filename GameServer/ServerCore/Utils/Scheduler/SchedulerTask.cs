using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Utils.Scheduler
{
    public class SchedulerTask
    {

        public SchedulerTask(long delay)
        {
            DelayInMs = delay;
            RunAt = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + delay;
        }

        public long DelayInMs;

        public long RunAt;

        public Action Task;

        public bool Repeat;
    }
}
