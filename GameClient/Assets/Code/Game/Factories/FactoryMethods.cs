using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class FactoryMethods
    {
        public static void AddCollider(GameObject obj)
        {
            var collider = obj.AddComponent<BoxCollider2D>();
            collider.offset = new Vector2(0.08f, 0.08f);
            collider.size = new Vector2(0.2f, 0.2f);
        }
    }
}
