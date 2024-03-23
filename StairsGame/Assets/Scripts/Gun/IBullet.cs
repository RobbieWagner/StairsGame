using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.ZombieStairs
{
    public interface IBullet
    {
        void KillZombie(Zombie zombie);
        void Launch(Vector2 direction);
    }
}
