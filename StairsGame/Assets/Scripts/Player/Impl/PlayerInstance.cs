using System.Collections;
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    [RequireComponent (typeof (I2DMovement))]
    public class PlayerInstance : MonoBehaviour, IStairsActor
    {
        [HideInInspector] public Stairs currentStairs = null;
        private int currentFlight;
        private int currentFloor;
        [HideInInspector] public bool isOnBackground = false;
        private PlayerMovement playerMovement;
        private AutoMovement autoMovement;

        public event IStairsActor.OnMoveBackwardForwardDelegate OnMoveBackwardForward;

        public static PlayerInstance Instance {get; private set;}

        private void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(gameObject); 
            else
                Instance = this; 

            playerMovement = GetComponentInChildren<PlayerMovement>();    
            autoMovement = GetComponentInChildren<AutoMovement>();
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

        public int CurrentFlight() => currentFlight;

        public int CurrentFloor() => currentFloor;

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
            //Debug.Log($"is player on stairs? {currentStairs != null}");
        }

        public Collider2D GetCollider()
        {
            return playerMovement != null ? playerMovement.GetComponentInChildren<Collider2D>() 
                    : autoMovement != null ? autoMovement.GetComponentInChildren<Collider2D>() 
                    : null;
        }

        // can be better (coupled)
        public void OnLandingReached()
        {
            if(CanMoveToBackground())
            {
                Debug.Log("Moved to BG");
                if(autoMovement != null)
                    StartCoroutine(autoMovement.MoveForwardBackward(false));
                currentFlight++;
            }
            else if(CanMoveToForeground())
            {
                Debug.Log("Moved to FG");
                if(autoMovement != null)
                    StartCoroutine(autoMovement.MoveForwardBackward(true));
                currentFlight++;
            }
        }
    }
}