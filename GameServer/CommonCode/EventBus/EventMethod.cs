using System;

namespace CommonCode.EventBus
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class EventMethod : Attribute
    {
    }
}
