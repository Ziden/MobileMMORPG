using MapHandler;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Game
{
    public class PlayerWrapper
    {
        public Position Position = new Position(0, 0);
        public int Speed;
        public string UserId;
        public string SessionId;
        public List<Position> FollowingPath;
        public GameObject PlayerObject;
        public GameObject Target;
    }
}
