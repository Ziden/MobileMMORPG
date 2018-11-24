using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Game.Monsters.Behaviours
{
    public class BehaviourPool
    {
        private static Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static BehaviourType Get<BehaviourType>() where BehaviourType : IMonsterBehaviour, new()
        {
            if(Instances.ContainsKey(typeof(BehaviourType)))
            {
                return (BehaviourType)Instances[typeof(BehaviourType)];
            } else
            {
                var instance = Activator.CreateInstance<BehaviourType>();
                Instances.Add(typeof(BehaviourType), instance);
                return instance;
            }
        }
    }
}
