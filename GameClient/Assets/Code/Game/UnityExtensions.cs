using Client.Net;
using MapHandler;
using UnityEngine;

namespace Assets.Code.Game
{
    public static class UnityExtensions
    {
        private static GameObject _entitiesContainer;

        public static Position GetMapPosition(this GameObject obj)
        {
            return new Position((int)obj.transform.position.x / 16, -(int)obj.transform.position.y / 16);
        }

        public static Vector2 ToUnityPosition(this Position p)
        {
            return new Vector2(p.X * 16, -p.Y * 16);
        }

        public static GameObject GetEntitiesContainer()
        {
            if (_entitiesContainer == null)
            {
                _entitiesContainer = GameObject.Find("Entities");
            }
            return _entitiesContainer;
        }

        public static LivingEntityBehaviour GetEntity(string entityId)
        {
           
            var entityGameObject = GetEntitiesContainer().transform.Find(entityId);
            if(entityGameObject != null)
            {
                if(entityId == UnityClient.Player.UID)
                {
                    return UnityClient.Player.Behaviour;
                }
                return entityGameObject.GetComponent<LivingEntityBehaviour>();
            }
            return null;
        }
    }
}
