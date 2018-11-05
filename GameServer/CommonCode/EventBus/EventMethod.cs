using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCode.EventBus
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class EventMethod : Attribute
    {
    }
}
