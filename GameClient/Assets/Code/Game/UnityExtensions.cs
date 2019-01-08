using Client.Net;
using CommonCode;
using MapHandler;
using UnityEngine;

namespace Assets.Code.Game
{
    public static class UnityExtensions
    {
        private static GameObject _entitiesContainer;

        public static Position GetMapPosition(this GameObject obj)
        {
            return new Position((int)obj.transform.position.x / GameCfg.TILE_SIZE_PIXELS, -(int)obj.transform.position.y / GameCfg.TILE_SIZE_PIXELS);
        }

        public static Vector2 ToUnityPosition(this Position p)
        {
            return new Vector2(p.X * GameCfg.TILE_SIZE_PIXELS, -p.Y * GameCfg.TILE_SIZE_PIXELS);
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
