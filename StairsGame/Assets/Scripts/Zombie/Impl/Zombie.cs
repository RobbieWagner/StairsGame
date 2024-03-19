using System.Collections;
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class Zombie : AutoMovement, IZombie, IStairsActor
    {
        [SerializeField] protected int maxHealth = 1;
        [SerializeField] protected ZombieState currentState = ZombieState.None;
        [SerializeField] private Collider2D zombieCollider;
        [HideInInspector] public bool isOnBackground = false;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        protected Stairs currentStairs;
        protected int flight;
        protected int floor => flight/2;
        private int SPRITE_FOREGROUND_LAYER = 2;
        private int SPRITE_BACKGROUND_LAYER = 0;
        
        protected bool movingUp = true;
        public bool canIgnoreStairDestruction = false;

        protected int health;
        public int Health
        {
            get {return health;}
            set
            {
                if(health == value) return;
                health = value;
                OnHealthChanged?.Invoke(health);
            }
        }
        public delegate void OnHealthChangedDelegate(int h);
        public event OnHealthChangedDelegate OnHealthChanged;
        public event IStairsActor.OnMoveBackwardForwardDelegate OnMoveBackwardForward;

        protected override void Awake()
        {
            health = maxHealth;

            canMove = true;
            movementSpeed = defaultWalkSpeed;
            running = false;

            currentRunSpeed = defaultRunSpeed;
            currentWalkSpeed = defaultWalkSpeed;

            //characterController.collisionMask = isOnBackground ? backgroundMask : foregroundMask;
        }

        protected override void FixedUpdate()
        {
            float direction = CurrentFlight() % 2 == 0 ? 1 : -1;
            velocity = new Vector2(direction * movementSpeed * Time.deltaTime, 
                                isGrounded ? -1f : - GRAVITY * Time.deltaTime);
            characterController.Move(velocity);
        }

        public void AttackPlayer(IStairsActor player)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(Stairs stairs)
        {
            if(stairs.isFlightValueEven() == Stairs.EVEN_FLIGHT_MEANS_FOREGROUND)
            {
                characterController.collisionMask = foregroundMask;
                StartCoroutine(MoveForwardBackward(true));
                isOnBackground = false;
            }
            else
            {
                characterController.collisionMask = backgroundMask;
                StartCoroutine(MoveForwardBackward(false));
                isOnBackground = true;
            }
        }

        public void StandIdle()
        {
            throw new System.NotImplementedException();
        }

        public void Wander()
        {
            throw new System.NotImplementedException();
        }

        public bool SetZombieSate(ZombieState state)
        {
            if(state == currentState) 
                return false;

            switch(state)
            {
                case ZombieState.Idle:
                    StandIdle();
                    break;
                case ZombieState.Wander:
                    Wander();
                    break;
                case ZombieState.Chasing:
                    ChasePlayer(PlayerInstance.Instance);
                    break;
                case ZombieState.Attacking:
                    AttackPlayer(PlayerInstance.Instance);
                    break;
                default:
                    break;
            }

            return true;
        }

        public void ChasePlayer(IStairsActor player)
        {
            if(transform.position.y > PlayerInstance.Instance.transform.position.y)
            {
                movingUp = false;
            }
        }

        public int CurrentFlight() => flight;

        public bool CanMoveToBackground() => currentStairs == null && !isOnBackground;

        public bool CanMoveToForeground() => currentStairs == null && isOnBackground;

        public int CurrentFloor() => floor;

        public void SetCurrentStairs(Stairs stairs)
        {
            if((currentStairs == null && stairs == null) ||
            (stairs != null && currentStairs != null && stairs.flightNumber == currentStairs.flightNumber)) 
                return;

            currentStairs = stairs;
            //Debug.Log($"is zombie on stairs? {currentStairs != null}");
        }

        public IEnumerator MoveToBackground()
        {
            yield return null;
            isOnBackground = true;
            spriteRenderer.sortingOrder = SPRITE_BACKGROUND_LAYER;
            spriteRenderer.color = new Color(.7f, .7f, .7f, 1f);
            OnMoveBackwardForward?.Invoke(this, false);
        }

        public IEnumerator MoveToForeground()
        {
            isOnBackground = false;
            yield return null;
            spriteRenderer.sortingOrder = SPRITE_FOREGROUND_LAYER;
            spriteRenderer.color = Color.white;
            OnMoveBackwardForward?.Invoke(this, true);
        }

        public Collider2D GetCollider() => zombieCollider;

        public void OnLandingReached()
        {
            if(CanMoveToBackground())
            {
                Debug.Log("Moved to BG");
                StartCoroutine(MoveForwardBackward(false));
                if(movingUp)
                    flight++;
                else
                    flight--;
            }
            else if(CanMoveToForeground())
            {
                Debug.Log("Moved to FG");
                StartCoroutine(MoveForwardBackward(true));
                if(movingUp)
                    flight++;
                else
                    flight--;
            }

            StairsManager.Instance.InstantiateNewStairs();
        }

        public override IEnumerator MoveForwardBackward(bool toForeground)
        {
            canMove = false;

            if(toForeground && CanMoveToForeground())
            {
                yield return StartCoroutine(MoveToForeground());
                characterController.collisionMask = foregroundMask;
            }
            else if (!toForeground && CanMoveToBackground())
            {
                yield return StartCoroutine(MoveToBackground());
                characterController.collisionMask = backgroundMask;
            }

            canMove = true;
            //forwardBackwardMovementCoroutine = null;
        }
    }
}