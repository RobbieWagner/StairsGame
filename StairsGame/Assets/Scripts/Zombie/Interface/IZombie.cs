using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.Player;

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
        public bool SetZombieSate(ZombieState state);
        public void StandIdle();
        public void Wander();
        public void ChasePlayer(IPlayerMovement player);
        public void AttackPlayer(IPlayerMovement player);
    }
}
