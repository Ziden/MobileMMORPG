using MapHandler;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Game
{
    public class PlayerWrapper : Entity
    {
        public int Speed;
        public string SessionId;
        public List<Position> FollowingPath;
        public GameObject PlayerObject;
        public GameObject Target;
    }
}
