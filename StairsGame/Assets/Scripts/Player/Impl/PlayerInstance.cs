using System.Collections;
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    [RequireComponent (typeof (PlayerMovement))]
    public class PlayerInstance : MonoBehaviour, IStairsActor
    {
        [HideInInspector] public Stairs currentStairs = null;
        [HideInInspector] public bool isOnBackground = false;
        private PlayerMovement playerMovement;

        public event IStairsActor.OnMoveBackwardForwardDelegate OnMoveBackwardForward;

        public static PlayerInstance Instance {get; private set;}

        private void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(gameObject); 
            else
                Instance = this; 

            playerMovement = GetComponentInChildren<PlayerMovement>();    
        }

        public bool CanMoveToBackground() => currentStairs == null && !isOnBackground;

        public bool CanMoveToForeground() => currentStairs == null && isOnBackground;

        public IEnumerator MoveToForeground()
        {
            isOnBackground = false;
            yield return null;
            OnMoveBackwardForward?.Invoke(this, true);
        }

        public IEnumerator MoveToBackground()
        {
            yield return null;
            isOnBackground = true;
            OnMoveBackwardForward?.Invoke(this, false);
        }

        public int CurrentFlight()
        {
            throw new System.NotImplementedException();
        }

        public int CurrentFloor()
        {
            throw new System.NotImplementedException();
        }

        public void KillPlayer()
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentStairs(Stairs stairs)
        {
            if((currentStairs == null && stairs == null) ||
            (stairs != null && currentStairs != null && stairs.flightNumber == currentStairs.flightNumber)) 
                return;

            currentStairs = stairs;
            Debug.Log($"is player on stairs? {currentStairs != null}");
        }

        public Collider2D GetCollider() => playerMovement.GetComponentInChildren<Collider2D>();
    }
}