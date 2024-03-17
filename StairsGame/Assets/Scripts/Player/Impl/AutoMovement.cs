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
        [SerializeField] private Controller2D characterController;
        [SerializeField] private LayerMask foregroundMask; 
        [SerializeField] private LayerMask backgroundMask; 
        [SerializeField] UnitAnimator unitAnimator;
        private const float GRAVITY = 15f;
        private bool moving;
        private Vector2 velocity;
        private float movementSpeed;
        public float defaultWalkSpeed = 3f;
        private float currentWalkSpeed;

        bool running;
        public float defaultRunSpeed = 6f;
        private float currentRunSpeed;

        public bool canMove;
        [SerializeField] public List<EventReference> movementSounds;

        bool isGrounded = false;

        Coroutine forwardBackwardMovementCoroutine = null;

        private void Awake() 
        {
            canMove = true;
            movementSpeed = defaultWalkSpeed;
            running = false;

            currentRunSpeed = defaultRunSpeed;
            currentWalkSpeed = defaultWalkSpeed;

            characterController.collisionMask = PlayerInstance.Instance.isOnBackground ? backgroundMask : foregroundMask;
        }

         private void FixedUpdate()
        {
            float direction = PlayerInstance.Instance.CurrentFlight() % 2 == 0 ? 1 : -1;
            velocity = new Vector2(direction * movementSpeed * Time.deltaTime, 
                                isGrounded ? -1f : - GRAVITY * Time.deltaTime);
            characterController.Move(velocity);
        }

        private void LateUpdate() => UpdateGroundCheck();

        private void UpdateGroundCheck()
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

        public IEnumerator MoveForwardBackward(bool toForeground)
        {
            canMove = false;

            if(toForeground && PlayerInstance.Instance.CanMoveToForeground())
            {
                yield return StartCoroutine(PlayerInstance.Instance.MoveToForeground());
                characterController.collisionMask = foregroundMask;
            }
            else if (PlayerInstance.Instance.CanMoveToBackground())
            {
                yield return StartCoroutine(PlayerInstance.Instance.MoveToBackground());
                characterController.collisionMask = backgroundMask;
            }

            canMove = true;
            forwardBackwardMovementCoroutine = null;
        }
    }
}