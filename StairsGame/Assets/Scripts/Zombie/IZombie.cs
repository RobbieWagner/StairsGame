using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public enum ZombieState
    {
        Idle,
        Wandering,
        Chasing,
        Attacking
    }

    public interface IZombie
    {
        public void Initialize();
        public void SetHealth();
        public bool SetZombieSate();
        public void StandIdle();
        public void Wander();
        public void ChasePlayer();
        public void AttackPlayer();
    }
}
