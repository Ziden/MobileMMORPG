using CommonCode.EventBus;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common
{
    public class EventBus<EventType>
    {
        private Dictionary<Type, List<RegisteredListener<EventType>>> _registeredListeners;

        public void RunCallbacks(EventType ev) {
            if (!_registeredListeners.ContainsKey(ev.GetType()))
                return;

            var registeredEvents = _registeredListeners[ev.GetType()];
            foreach(var registeredEvent in registeredEvents)
            {
                registeredEvent.Call(ev);
            }
        }

        public EventBus()
        {
            _registeredListeners = new Dictionary<Type, List<RegisteredListener<EventType>>>();
        }

        private void RegisterCallback(IEventListener listener, MethodInfo method, Type eventType)
        {
            if(!_registeredListeners.ContainsKey(eventType))
            {
                _registeredListeners.Add(eventType, new List<RegisteredListener<EventType>>());
            }
            var eventList = _registeredListeners[eventType];
            eventList.Add(new RegisteredListener<EventType>()
            {
                Method = method,
                Listener = listener
            });
        }

        public void RegisterListener(IEventListener listener)
        {
            var type = listener.GetType();
            var methods = type.GetMethods();
            foreach (var method in type.GetMethods())
            {
                var parameters = method.GetParameters();
                if(parameters.Length == 1)
                {
                    var parameter = parameters[0];
                    if (typeof(EventType).IsAssignableFrom(parameter.ParameterType)) {
                        RegisterCallback(listener, method, parameter.ParameterType);
                    }
                }
            }
        }
    }
}
