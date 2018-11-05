using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Tests.TestUtilities
{
    public static class Waiter
    {
        public static void WaitUntil(Func<bool> condition, int timeout = 1000)
        {
            var time = DateTime.Now.Millisecond;
            var wait = !condition.Invoke();
            while (wait)
            {
                var now = DateTime.Now.Millisecond;
                if (now < time + 1000)
                {
                    wait = !condition.Invoke();
                } else
                {
                    wait = false;
                }
            }
        }
    }
}
