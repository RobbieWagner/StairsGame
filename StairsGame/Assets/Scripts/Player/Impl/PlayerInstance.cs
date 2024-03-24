using System;
using System.Collections;
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    [RequireComponent (typeof (I2DMovement))]
    public class PlayerInstance : MonoBehaviour, IStairsActor
    {
        [HideInInspector] public Stairs currentStairs = null;
        [SerializeField] private SpriteRenderer playerSprite; 
        private int currentFlight;
        private int currentFloor;
        [HideInInspector] public bool isOnBackground = false;
        private PlayerMovement playerMovement;
        private AutoMovement autoMovement;
        public bool canDie = true;

        public event IStairsActor.OnMoveBackwardForwardDelegate OnMoveBackwardForward;

        private int PLAYER_BACKGROUND_LAYER = 2;
        private int PLAYER_FOREGROUND_LAYER = 7;

        public static PlayerInstance Instance {get; private set;}

        private void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(gameObject); 
            else
                Instance = this; 

            playerMovement = GetComponentInChildren<PlayerMovement>();    
            autoMovement = GetComponentInChildren<AutoMovement>();
            autoMovement.canMove = false;
            PlayerGun.Instance.gunRenderer.sortingOrder = PLAYER_FOREGROUND_LAYER + 1;
        }

        public bool CanMoveToBackground() => currentStairs == null && !isOnBackground;

        public bool CanMoveToForeground() => currentStairs == null && isOnBackground;

        public IEnumerator MoveToForeground()
        {
            isOnBackground = false;
            yield return null;
            playerSprite.sortingOrder = PLAYER_FOREGROUND_LAYER;
            PlayerGun.Instance.gunRenderer.sortingOrder = PLAYER_FOREGROUND_LAYER + 1;
            playerSprite.color = Color.white;
            OnMoveBackwardForward?.Invoke(this, true);
        }

        public IEnumerator MoveToBackground()
        {
            yield return null;
            isOnBackground = true;
            playerSprite.sortingOrder = PLAYER_BACKGROUND_LAYER;
            PlayerGun.Instance.gunRenderer.sortingOrder = PLAYER_BACKGROUND_LAYER + 1;
            playerSprite.color = new Color(.7f, .7f, .7f, 1f);
            OnMoveBackwardForward?.Invoke(this, false);
        }

        public int CurrentFlight() => currentFlight;

        public int CurrentFloor() => currentFloor;

        public bool KillPlayer()
        {
            if(canDie)
            {
                autoMovement.canMove = false;
                return true;
            }
            return false;
        }

        public void SetCurrentStairs(Stairs stairs)
        {
            if((currentStairs == null && stairs == null) ||
            (stairs != null && currentStairs != null && stairs.flightNumber == currentStairs.flightNumber)) 
                return;

            currentStairs = stairs;
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
            //Debug.Log($"is on bg {isOnBackground} can move {currentStairs == null}");
            if(CanMoveToBackground())
            {
                //Debug.Log("Moved to BG");
                if(autoMovement != null)
                    StartCoroutine(autoMovement.MoveForwardBackward(false));
                currentFlight++;
            }
            else if(CanMoveToForeground())
            {
                //Debug.Log("Moved to FG");
                if(autoMovement != null)
                    StartCoroutine(autoMovement.MoveForwardBackward(true));
                currentFlight++;
            }

            StairsManager.Instance.InstantiateNewStairs();
        }

        public void Initialize()
        {
            autoMovement.canMove = true;
        }
    }
}