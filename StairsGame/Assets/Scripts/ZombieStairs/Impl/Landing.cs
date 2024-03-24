
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class Landing : MonoBehaviour, ILanding
    {
        //child classes may want to give the player a choice about where to go next
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            IStairsActor stairsActor = other.GetComponentInChildren<IStairsActor>();
            if(stairsActor != null)
            {
                stairsActor.OnLandingReached();

                PlayerInstance playerInstance = other.GetComponentInChildren<PlayerInstance>();
                if(playerInstance != null)
                    GameManager.Instance.Score += GameManager.newFloorReached;
            }
        }
    }
}
