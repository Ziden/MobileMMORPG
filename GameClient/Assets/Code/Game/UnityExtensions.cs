using MapHandler;
using UnityEngine;

namespace Assets.Code.Game
{
    public static class UnityExtensions
    {
        public static Position GetMapPosition(this GameObject obj)
        {
            return new Position((int)obj.transform.position.x / 16, -(int)obj.transform.position.y / 16);
        }

        public static Vector2 ToUnityPosition(this Position p)
        {
            return new Vector2(p.X * 16, -p.Y * 16);
        }
    }
}
