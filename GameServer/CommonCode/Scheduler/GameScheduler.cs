using System;
using System.Collections.Generic;

namespace Common.Scheduler
{
    public class GameScheduler
    {
        public static Dictionary<Guid, SchedulerTask> TaskByIds = new Dictionary<Guid, SchedulerTask>();
        public static List<SchedulerTask> Tasks = new List<SchedulerTask>();

        public static void RunTasks(long timeNow)
        {
            if (Tasks.Count == 0)
                return;
            var task = Tasks[0];
            var taskWasDelayedBy = timeNow - task.RunAt + 1;
            if (taskWasDelayedBy >= 0)
            {
              
                if(!task.Repeat)
                {
                    TaskByIds.Remove(task.UID);
                    Tasks.RemoveAt(0);
                }   
                    
                else
                {
                    task.RunAt = timeNow + task.DelayInMs;
                    Schedule(task);
                }
                task.Task.Invoke();
                RunTasks(timeNow);
            } 
        }

        public static SchedulerTask GetTask(Guid id)
        {
            if (TaskByIds.ContainsKey(id))
            {
                return TaskByIds[id];
            }
            return null;
        }

        public static void CancelTask(Guid id)
        {
            if(TaskByIds.ContainsKey(id))
            {
                var task = TaskByIds[id];
                Tasks.Remove(task);
                TaskByIds.Remove(id);
            } 
        }

        public static Guid Schedule(SchedulerTask newTask)
        {
            for(var i = 0; i < Tasks.Count; i++)
            {
                var task = Tasks[i];
                // does the new task runs before ?
                if(newTask.RunAt < task.RunAt)
                {
                    Tasks.Insert(i, newTask);
                    TaskByIds.Add(newTask.UID, newTask);
                    return newTask.UID;
                }
            }
            // If its after all tasks
            Tasks.Insert(Tasks.Count, newTask);
            TaskByIds.Add(newTask.UID, newTask);
            return newTask.UID;
        }
    }
}
