using ServerCore.Game.Monsters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Game.GameMap.MobSpawners
{
    public class SpawnerMob
    {
        public Type MonsterClassType;
        public int Amount;

        public Monster TrackedMonster;
    }
}
