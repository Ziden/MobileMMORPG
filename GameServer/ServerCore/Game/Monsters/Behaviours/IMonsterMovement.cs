using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Game.Monsters.Behaviours
{
    public interface IMonsterMovement : IMonsterBehaviour
    {
        void PerformMovement(Monster monster);
    }
}
