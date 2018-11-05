using NUnit.Framework;
using System;
using ServerCore.Utils.Scheduler;
using System.Collections.Generic;

[TestFixture]
public class TestScheduler
{

    [Test]
    // Test to ensure server is recieving the packets and keeping the recieve order
    public void TestBasicScheduling()
    {

        List<int> executedTasks = new List<int>();

        var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        GameScheduler.Schedule(new SchedulerTask(1000)
        {
            Task = () =>
            {
                executedTasks.Add(1);
            }
        });

        GameScheduler.Schedule(new SchedulerTask(2000)
        {
            Task = () =>
            {
                executedTasks.Add(2);
            }
        });

        GameScheduler.Schedule(new SchedulerTask(3000)
        {
            Task = () =>
            {
                executedTasks.Add(3);
            }
        });

        GameScheduler.Schedule(new SchedulerTask(1500)
        {
            Task = () =>
            {
                executedTasks.Add(4);
            }
        });

        GameScheduler.RunTasks(now);

        var tasks = GameScheduler.Tasks;

        Assert.That(executedTasks.Count == 0);

        GameScheduler.RunTasks(now + 1500);

        Assert.That(executedTasks.Contains(4));
        Assert.That(executedTasks.Contains(1));
        Assert.That(executedTasks.Count == 2);

        GameScheduler.RunTasks(now + 3000);

        Assert.That(executedTasks.Contains(2));
        Assert.That(executedTasks.Contains(3));
        Assert.That(executedTasks.Count == 4);
        Assert.That(GameScheduler.Tasks.Count == 0);

    }

    [Test]
    // Test to ensure server is recieving the packets and keeping the recieve order
    public void TestLoopingSchedule()
    {
        List<int> executedTasks = new List<int>();

        var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        GameScheduler.Schedule(new SchedulerTask(1000)
        {
            Repeat = true,
            Task = () =>
            {
                executedTasks.Add(1);
            }
        });

        GameScheduler.RunTasks(now + 1000);
        GameScheduler.RunTasks(now + 2000);
        GameScheduler.RunTasks(now + 3000);
        GameScheduler.RunTasks(now + 3500);

        Assert.That(executedTasks.Count == 3);

    }

}

