
using System;
using System.Collections.Generic;

namespace Common.Entity
{
    public abstract class LivingEntity : Entity
    {
        public string Name;
        public int HP = 10;
        public int MAXHP = 10;
        public int Atk = 3;
        public int Def = 1;
        public int AtkSpeed = 5;

        public long NextAttackAt = 0;

        // When a living entity is targeting another to attack
        public LivingEntity Target;
        public List<LivingEntity> BeingTargetedBy = new List<LivingEntity>();

        public Guid AttackTaskId;

        public abstract void Die();

        public virtual void SetTarget(LivingEntity entity) {
            this.Target = entity;
            entity.BeTargeted(this);
        }

        public virtual void BeTargeted(LivingEntity targetter)
        {
            targetter.BeingTargetedBy.Add(this);
        }
    }
}
