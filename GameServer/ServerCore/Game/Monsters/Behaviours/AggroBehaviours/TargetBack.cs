using System;
using System.Collections.Generic;
using System.Text;
using Common.Entity;
using ServerCore.GameServer.Players.Evs;

namespace ServerCore.Game.Monsters.Behaviours.AggroBehaviours
{
    public class TargetBack : IMonsterAggro
    {
        public void OnBeingTargeted(Monster monster, LivingEntity targetting)
        {
            Server.Events.Call(new EntityTargetEvent()
            {
                Entity = monster,
                TargetedEntity = targetting
            });
        }
    }
}
