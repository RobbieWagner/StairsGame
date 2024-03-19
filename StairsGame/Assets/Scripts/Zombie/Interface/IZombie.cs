using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.Player;

namespace RobbieWagnerGames.ZombieStairs
{
    public enum ZombieState
    {
        None = -1,
        Idle = 0,
        Wander = 1,
        Chasing = 2,
        Attacking = 3
    }

    public interface IZombie
    {
        public void Initialize(Stairs stairs);
        public bool SetZombieSate(ZombieState state);
        public void StandIdle();
        public void Wander();
        public void ChasePlayer(IStairsActor player);
        public void AttackPlayer(IStairsActor player);
    }
}
