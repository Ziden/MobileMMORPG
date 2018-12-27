using MapHandler;
using UnityEngine;

namespace Assets.Code.Game
{
    public class PlayerWrapper : Entity
    {
        public int Speed;
        public string SessionId;

        public GameObject PlayerObject;
        public GameObject Target;
        public MovingEntityBehaviour Movement;
    }
}
