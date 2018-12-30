
namespace Common.Entity
{
    public class LivingEntity : Entity
    {
        public string Name;
        public int HP = 10;
        public int Atk = 100;
        public int Def = 1;
        public int AtkSpeed = 5;

        // When a living entity is targeting another to attack
        public LivingEntity Target;
    }
}
