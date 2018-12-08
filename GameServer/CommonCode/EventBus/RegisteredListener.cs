using System.Reflection;

namespace CommonCode.EventBus
{
    public class RegisteredListener<EventType>
    {
        public IEventListener Listener;
        public MethodInfo Method;

        public void Call(EventType ev)
        {
            Method.Invoke(Listener, new object[] { ev });
        }
    }
}
