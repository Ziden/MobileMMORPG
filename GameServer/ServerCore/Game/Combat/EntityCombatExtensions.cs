using Common.Entity;
using Common.Scheduler;
using CommonCode.EntityShared;
using MapHandler;
using ServerCore.GameServer.Players.Evs;

namespace ServerCore.Game.Combat
{
    public static class EntityCombatExtensions
    {
        public static void TryAttacking(this LivingEntity attacker, LivingEntity defender, bool singleHit = false)
        {
            if(attacker.Position.GetDistance(defender.Position) > 1)
            {
                Log.Info("Entity tryed to attack other entity being too far away");
                return;
            }
            if(defender.HP <= 0 || attacker.HP < 0)
            {
                return;
            }
            var now = GameThread.TIME_MS_NOW;
            if(now < attacker.NextAttackAt)
            {
                var timeToFinishCd = attacker.NextAttackAt - now;
                if(!singleHit)
                    RescheduleAttack(attacker, defender, timeToFinishCd);
                return;
            }
            if (attacker.AttackTaskId != null)
            {
                GameScheduler.CancelTask(attacker.AttackTaskId);
            }
            var attackDelay = Formulas.GetTimeBetweenAttacks(attacker.AtkSpeed);
            attacker.NextAttackAt = now + attackDelay;

            Log.Debug(attacker.Name + " Attacking " + defender.Name);
            Server.Events.Call(new EntityAttackEvent()
            {
                Attacker = attacker,
                Defender = defender
            });

            if(!singleHit)
                RescheduleAttack(attacker, defender, attackDelay);
        }

        public static void RescheduleAttack(LivingEntity attacker, LivingEntity defender, long attackDelay)
        {
            var attackTask = new SchedulerTask(attackDelay + 1, GameThread.TIME_MS_NOW)
            {
                Task = () =>
                {
                    if (attacker.EntityType == EntityType.PLAYER)
                    {
                        if (Server.GetPlayer(attacker.UID) == null)
                        {
                            return;
                        }
                    }
                    TryAttacking(attacker, defender);
                }
            };

            GameScheduler.Schedule(attackTask);
            attacker.AttackTaskId = attackTask.UID;
        }
    }
}
