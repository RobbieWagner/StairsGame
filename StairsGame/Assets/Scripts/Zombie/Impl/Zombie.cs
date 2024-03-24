using System.Collections;
using System.Linq;
using RobbieWagnerGames.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobbieWagnerGames.ZombieStairs
{
    public class Zombie : AutoMovement, IZombie, IStairsActor
    {
        [SerializeField] protected int maxHealth = 1;
        [SerializeField] protected ZombieState currentState = ZombieState.None;
        [SerializeField] private Collider2D zombieCollider;
        [HideInInspector] public bool isOnBackground = false;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Rigidbody2D rb2d;
        [SerializeField] public int killScore = 50;
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
            float direction = CurrentFlight() % 2 == 0 
                ? PlayerInstance.Instance.CurrentFlight() > CurrentFlight() 
                    ? 1 : PlayerInstance.Instance.CurrentFlight() == CurrentFlight() 
                        ? PlayerInstance.Instance.transform.position.y > transform.position.y 
                            ? 1 : -1 : -1
                : PlayerInstance.Instance.CurrentFlight() > CurrentFlight() 
                    ? -1 : PlayerInstance.Instance.CurrentFlight() == CurrentFlight() 
                        ? PlayerInstance.Instance.transform.position.y > transform.position.y 
                            ? -1 : 1 : 1;
                            
            //Debug.Log(CurrentFlight());
            velocity = new Vector2(direction * movementSpeed * Time.deltaTime, 
                                isGrounded ? -1f : - GRAVITY * Time.deltaTime);
            characterController.Move(velocity);
        }

        public void AttackPlayer(IStairsActor player)
        {
            SceneManager.LoadScene("Game");
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if(other.collider.GetComponent<PlayerInstance>() != null)
                SetZombieSate(ZombieState.Attacking);
        }

        public void Initialize(Stairs stairs)
        {
            if(stairs.isFlightValueEven() == Stairs.EVEN_FLIGHT_MEANS_FOREGROUND)
            {
                //Debug.Log("zombie is foreground");
                characterController.collisionMask = foregroundMask;
                StartCoroutine(MoveForwardBackward(true));
                isOnBackground = false;
            }
            else
            {
                //Debug.Log("zombie is background");
                characterController.collisionMask = backgroundMask;
                StartCoroutine(MoveForwardBackward(false));
                isOnBackground = true;
            }
            flight = stairs.flightNumber;
        }

        public virtual void StandIdle()
        {
            Debug.LogWarning("StandIdle not implemented");
        }

        public virtual void Wander()
        {
            Debug.LogWarning("Wander not implemented");
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
            //Debug.Log("BG");
        }

        public IEnumerator MoveToForeground()
        {
            isOnBackground = false;
            yield return null;
            spriteRenderer.sortingOrder = SPRITE_FOREGROUND_LAYER;
            spriteRenderer.color = Color.white;
            OnMoveBackwardForward?.Invoke(this, true);
            //Debug.Log("FG");
        }

        public Collider2D GetCollider() => zombieCollider;

        public void OnLandingReached()
        {
            if(CanMoveToBackground())
            {
                //Debug.Log("Moved to BG");
                StartCoroutine(MoveForwardBackward(false));
                if(movingUp)
                    flight++;
                else
                    flight--;
            }
            else if(CanMoveToForeground())
            {
                //Debug.Log("Moved to FG");
                StartCoroutine(MoveForwardBackward(true));
                if(movingUp)
                    flight++;
                else
                    flight--;
            }
        }

        public override IEnumerator MoveForwardBackward(bool toForeground)
        {
            canMove = false;

            if(toForeground && CanMoveToForeground())
            {
                yield return StartCoroutine(MoveToForeground());
                characterController.collisionMask = foregroundMask;
                rb2d.includeLayers = foregroundMask;
                rb2d.excludeLayers = ignoreBackgroundMask;
            }
            else if (!toForeground && CanMoveToBackground())
            {
                yield return StartCoroutine(MoveToBackground());
                characterController.collisionMask = backgroundMask;
                rb2d.includeLayers = backgroundMask;
                rb2d.excludeLayers = ignoreForegroundMask;
            }

            canMove = true;
            //forwardBackwardMovementCoroutine = null;
        }

        protected override void UpdateGroundCheck()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2) transform.position, Vector2.down, .25f, isOnBackground? backgroundMask : foregroundMask);
            //Debug.DrawLine(transform.position, transform.position + Vector3.down * .25f, Color.green);
            if(hits.Any())
            {
                isGrounded = true;
                var stairs = hits.Select(hit => hit.collider.GetComponentInChildren<Stairs>()).Where(flight => flight != null); 
                //Debug.Log($"number of found stairs {stairs.Count()}");
                SetCurrentStairs(stairs.Any() ? stairs.First() : null);
            }
            else
            {
                isGrounded = false;
                SetCurrentStairs(null);
            }
        }
    }
}