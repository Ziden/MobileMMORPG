using System;

namespace Common.Scheduler
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

        public Guid UID = Guid.NewGuid();
    }
}
