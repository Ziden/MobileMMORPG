using System;

namespace Common.Scheduler
{
    public class SchedulerTask
    {

        public SchedulerTask(long delay, long timeNow)
        {
            DelayInMs = delay;
            RunAt = timeNow + delay;
        }

        public long DelayInMs;

        public long RunAt;

        public Action Task;

        public bool Repeat;

        public Guid UID = Guid.NewGuid();
    }
}
