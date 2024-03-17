using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames.ZombieStairs;
using UnityEngine;
namespace RobbieWagnerGames.Player
{
    public interface IStairsActor
    {
        int CurrentFlight();
        int CurrentFloor();
        bool CanMoveToBackground();
        bool CanMoveToForeground();
        IEnumerator MoveToBackground();
        IEnumerator MoveToForeground();
        Collider2D GetCollider();

        void SetCurrentStairs(Stairs stairs);
        public delegate void OnMoveBackwardForwardDelegate(IStairsActor actor, bool toForeground);
        public event OnMoveBackwardForwardDelegate OnMoveBackwardForward;
    }
}
