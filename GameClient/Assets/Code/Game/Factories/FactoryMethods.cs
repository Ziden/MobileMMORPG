using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public enum FactoryObjectTypes {
        TILE = 1,
        PLAYER = 2,
        MONSTER = 3
    }

    public class FactoryMethods
    {
        public static FactoryObjectTypes GetType(GameObject obj)
        {
            if (obj.name.StartsWith("mon"))
            {
                return FactoryObjectTypes.MONSTER;
            } else if (obj.tag == "player")
            {
                return FactoryObjectTypes.PLAYER;
            }
            return FactoryObjectTypes.TILE;
        }

        public static void AddCollider(GameObject obj)
        {
            var collider = obj.AddComponent<BoxCollider2D>();
            collider.offset = new Vector2(0.08f, 0.08f);
            collider.size = new Vector2(0.2f, 0.2f);
        }
    }
}
