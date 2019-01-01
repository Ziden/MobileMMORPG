
namespace Common.Entity
{
    public static class EntityCommonExtensions
    {
        public static EntityType GetEntityTypeFromUid(this string uid)
        {
            if(uid.StartsWith("mon"))
            {
                return EntityType.MONSTER;
            }
            return EntityType.PLAYER;
        }
    }
}
