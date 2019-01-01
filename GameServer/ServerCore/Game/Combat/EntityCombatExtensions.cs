using Common.Entity;
using Common.Scheduler;
using CommonCode.EntityShared;
using MapHandler;
using ServerCore.GameServer.Players.Evs;

namespace ServerCore.Game.Combat
{
    public static class EntityCombatExtensions
    {
        public static void TryAttacking(this LivingEntity attacker, LivingEntity defender)
        {
            if(attacker.Position.GetDistance(defender.Position) > 1)
            {
                Log.Info("Entity tryed to attack other entity being too far away");
                return;
            }
            var now = GameThread.TIME_MS_NOW;
            if(now < attacker.NextAttackAt)
            {
                var timeToFinishCd = attacker.NextAttackAt - now;
                RescheduleAttack(attacker, defender, timeToFinishCd);
                Log.Debug("Player attack cooldown");
                return;
            }

            var attackDelay = Formulas.GetTimeBetweenAttacks(attacker.AtkSpeed);
            attacker.NextAttackAt = now + attackDelay;

            var damage = 2; // testing

            Server.Events.Call(new EntityAttackEvent()
            {
                Damage = damage,
                Attacker = attacker,
                Defender = defender
            });

            RescheduleAttack(attacker, defender, attackDelay);


        }

        public static void RescheduleAttack(LivingEntity attacker, LivingEntity defender, long attackDelay)
        {
            if (attacker.AttackTaskId != null)
            {
                GameScheduler.CancelTask(attacker.AttackTaskId);
            }

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
            Log.Info("SCHEDULED NEW TASK");
            attacker.AttackTaskId = attackTask.UID;
        }
    }
}
