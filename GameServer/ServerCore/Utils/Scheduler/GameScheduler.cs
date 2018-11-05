using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Utils.Scheduler
{
    public class GameScheduler
    {
        public static List<SchedulerTask> Tasks = new List<SchedulerTask>();

        public static void RunTasks(long timeNow)
        {
            if (Tasks.Count == 0)
                return;
            var task = Tasks[0];
            var delay = timeNow - task.RunAt + 1;
            if (delay >= 0)
            {
                task.Task.Invoke();
                if(!task.Repeat)
                    Tasks.RemoveAt(0);
                else
                {
                    task.RunAt = timeNow + task.DelayInMs;
                    Schedule(task);
                }
                RunTasks(timeNow);
            }
            if(delay > 0)
            {
                Log.Debug("Task delay: " + delay);
            }
        }

        public static void Schedule(SchedulerTask newTask)
        {
            for(var i = 0; i < Tasks.Count; i++)
            {
                var task = Tasks[i];
                // does the new task runs before ?
                if(newTask.RunAt < task.RunAt)
                {
                    Tasks.Insert(i, newTask);
                    return;
                }
            }
            // If its after all tasks
            Tasks.Insert(Tasks.Count, newTask);
        }
    }
}
