using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class AutoMovement : MonoBehaviour, I2DMovement
    {
        [SerializeField] protected Controller2D characterController;
        [SerializeField] protected LayerMask foregroundMask; 
        [SerializeField] protected LayerMask ignoreBackgroundMask;
        [SerializeField] protected LayerMask backgroundMask; 
        [SerializeField] protected LayerMask ignoreForegroundMask;
        [SerializeField] UnitAnimator unitAnimator;
        protected const float GRAVITY = 15f;
        private bool moving;
        protected Vector2 velocity;
        protected float movementSpeed;
        public float defaultWalkSpeed = 3f;
        protected float currentWalkSpeed;

        protected bool running;
        public float defaultRunSpeed = 6f;
        protected float currentRunSpeed;

        public bool canMove = true;
        [SerializeField] public List<EventReference> movementSounds;

        protected bool isGrounded = false;

        Coroutine forwardBackwardMovementCoroutine = null;

        protected virtual void Awake() 
        {
            canMove = true;
            movementSpeed = defaultWalkSpeed;
            running = false;

            currentRunSpeed = defaultRunSpeed;
            currentWalkSpeed = defaultWalkSpeed;

            characterController.collisionMask = PlayerInstance.Instance.isOnBackground ? backgroundMask : foregroundMask;
        }

        protected virtual void FixedUpdate()
        {
            float direction = PlayerInstance.Instance.CurrentFlight() % 2 == 0 ? 1 : -1;
            velocity = new Vector2(direction * movementSpeed * Time.deltaTime, 
                                isGrounded ? -1f : - GRAVITY * Time.deltaTime);
            if(canMove)
                characterController.Move(velocity);
        }

        protected virtual void LateUpdate() => UpdateGroundCheck();

        protected virtual void UpdateGroundCheck()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2) transform.position, Vector2.down, .25f, PlayerInstance.Instance.isOnBackground? backgroundMask : foregroundMask);
            //Debug.DrawLine(transform.position, transform.position + Vector3.down * .25f, Color.green);
            if(hits.Any())
            {
                isGrounded = true;
                var stairs = hits.Select(hit => hit.collider.GetComponentInChildren<Stairs>()).Where(flight => flight != null); 
                //Debug.Log($"number of found stairs {stairs.Count()}");
                PlayerInstance.Instance.SetCurrentStairs(stairs.Any() ? stairs.First() : null);
            }
            else
            {
                isGrounded = false;
                PlayerInstance.Instance.SetCurrentStairs(null);
            }
            //Debug.Log($"is grounded? {isGrounded}");
        }

        public void ChangeSpeed(float newWalkSpeed, float newRunSpeed)
        {
            throw new System.NotImplementedException();
        }

        public void ResetSpeeds()
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerator MoveForwardBackward(bool toForeground)
        {
            canMove = false;

            if(toForeground && PlayerInstance.Instance.CanMoveToForeground())
            {
                yield return StartCoroutine(PlayerInstance.Instance.MoveToForeground());
                characterController.collisionMask = foregroundMask;
            }
            else if (!toForeground && PlayerInstance.Instance.CanMoveToBackground())
            {
                yield return StartCoroutine(PlayerInstance.Instance.MoveToBackground());
                characterController.collisionMask = backgroundMask;
            }

            canMove = true;
            forwardBackwardMovementCoroutine = null;
        }
    }
}