using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Networking
{
    public class PacketSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        public PacketSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
        public void Switch(object x) { matches[x.GetType()](x); }
    }
}
