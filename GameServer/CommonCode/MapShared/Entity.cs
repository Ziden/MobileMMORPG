using ServerCore.Game.Entities;
using System;

namespace MapHandler
{
    [Serializable]
    public abstract class Entity
    {
        public string UID;
        public Position Position;
        public Position LastPosition;
        public EntityType EntityType;
        public int MoveSpeed = 5;
    }
}
