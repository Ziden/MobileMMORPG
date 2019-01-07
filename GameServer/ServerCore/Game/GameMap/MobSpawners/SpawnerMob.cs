using ServerCore.Game.Monsters;
using System;

namespace ServerCore.Game.GameMap.MobSpawners
{
    public class SpawnerMob
    {
        public Type MonsterClassType;
        public int Amount;

        public Monster TrackedMonster;
    }
}
