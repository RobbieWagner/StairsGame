using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames.Player;
using UnityEngine.InputSystem;
using FMODUnity;
using System.Linq;

namespace RobbieWagnerGames.ZombieStairs
{
    public class PlayerMovement : MonoBehaviour, I2DMovement
    {
        //[SerializeField] Rigidbody2D body;
        [SerializeField] private Controller2D characterController;
        [SerializeField] private LayerMask foregroundMask; 
        [SerializeField] private LayerMask backgroundMask; 
        [SerializeField] UnitAnimator unitAnimator;
        private MovementControls playerMovementControls;
        private const float GRAVITY = 15f;

        private float movementInput;
        private bool moving;
        private Vector2 velocity;

        private float movementSpeed;
        public float defaultWalkSpeed = 3f;
        private float currentWalkSpeed;

        bool running;
        public float defaultRunSpeed = 6f;
        private float currentRunSpeed;

        public bool canMove;
        [HideInInspector] public bool hasRecentlyMoved;

        [SerializeField] public List<EventReference> movementSounds;

        bool isGrounded = false;

        Coroutine forwardBackwardMovementCoroutine = null;


        private void Awake() 
        {
            canMove = true;
            movementSpeed = defaultWalkSpeed;
            running = false;
            hasRecentlyMoved = false;

            currentRunSpeed = defaultRunSpeed;
            currentWalkSpeed = defaultWalkSpeed;

            characterController.collisionMask = PlayerInstance.Instance.isOnBackground ? backgroundMask : foregroundMask;

            playerMovementControls = new MovementControls();
            playerMovementControls.Enable();

            playerMovementControls.Movement.Move.performed += OnMove;
            playerMovementControls.Movement.Move.canceled += OnStopMoving;
            playerMovementControls.Movement.Run.performed += OnStartRun;
            playerMovementControls.Movement.Run.canceled += OnStopRun;

            playerMovementControls.Movement.EnterForeground.performed += OnEnterForeground;
            playerMovementControls.Movement.EnterBackground.performed += OnEnterBackground;

            playerMovementControls.Movement.Interact.performed += OnInteract;
        }

        private void FixedUpdate()
        {
            velocity = new Vector2(movementInput * movementSpeed * Time.deltaTime, isGrounded ? -1f : - GRAVITY * Time.deltaTime);
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

        private void OnMove(InputAction.CallbackContext context)
        {
            if(canMove)
            {
                float input = context.ReadValue<float>();

                if(movementInput != input && input != 0f)
                {
                    if(input > 0) 
                    {
                        if(running)
                            unitAnimator.ChangeAnimationState(UnitAnimationState.RunRight);
                        else
                            unitAnimator.ChangeAnimationState(UnitAnimationState.WalkRight);
                    }
                    else 
                    {
                        if(running)
                            unitAnimator.ChangeAnimationState(UnitAnimationState.RunLeft);
                        else
                            unitAnimator.ChangeAnimationState(UnitAnimationState.WalkLeft);
                    }
                    moving = true;
                }
                
                movementInput = input;
            }
        }

        private void OnStopMoving(InputAction.CallbackContext context)
        {
            if(movementInput > 0)
                unitAnimator.ChangeAnimationState(UnitAnimationState.IdleRight);
            else if(movementInput < 0)
                unitAnimator.ChangeAnimationState(UnitAnimationState.IdleLeft);
            else if(movementInput > 0) 
                unitAnimator.ChangeAnimationState(UnitAnimationState.IdleForward);
            else 
                unitAnimator.ChangeAnimationState(UnitAnimationState.Idle);

            moving = false;
            //movementSounds?.ToggleMovementSounds(false);

            movementInput = 0;
        }

        public void OnEnterForeground(InputAction.CallbackContext context)
        {
            forwardBackwardMovementCoroutine = StartCoroutine(MoveForwardBackward(true));
            //Debug.Log("move to fg");
        }

        public void OnEnterBackground(InputAction.CallbackContext context)
        {
            forwardBackwardMovementCoroutine = StartCoroutine(MoveForwardBackward(false));
            //Debug.Log("move to bg");
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

        public void OnStartRun(InputAction.CallbackContext context)
        {
            running = true;
            //movementSounds?.ToggleRun(true);
            movementSpeed = currentRunSpeed;

            if(moving)
            {
                if(Mathf.Abs(movementInput) > Mathf.Abs(movementInput))
                {
                    if(movementInput > 0) 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.RunRight);
                    else 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.RunLeft);
                }
                else
                {
                    if(movementInput > 0)
                        unitAnimator.ChangeAnimationState(UnitAnimationState.RunForward);
                    else  
                        unitAnimator.ChangeAnimationState(UnitAnimationState.RunBack);
                }
            }
        }

        public void OnStopRun(InputAction.CallbackContext context)
        {
            running = false;
            //movementSounds?.ToggleRun(false);
            movementSpeed = currentWalkSpeed;

            if(moving)
            {
                if(Mathf.Abs(movementInput) > Mathf.Abs(movementInput))
                {
                    if(movementInput > 0) 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.WalkRight);
                    else 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.WalkLeft);
                }
                else
                {
                    if(movementInput > 0)
                        unitAnimator.ChangeAnimationState(UnitAnimationState.WalkForward);
                    else  
                        unitAnimator.ChangeAnimationState(UnitAnimationState.WalkBack);
                }
            }
        }

        public void ChangeSpeed(float newWalkSpeed, float newRunSpeed)
        {
            currentWalkSpeed = newWalkSpeed;
            currentRunSpeed = newRunSpeed;

            if(running) 
                movementSpeed = currentRunSpeed;
            else 
                movementSpeed = currentWalkSpeed;
        }

        public void ResetSpeeds()
        {
            currentRunSpeed = defaultRunSpeed;
            currentWalkSpeed = defaultWalkSpeed;

            if(running) 
                movementSpeed = currentRunSpeed;
            else 
                movementSpeed = currentWalkSpeed;
        }
        private void OnInteract(InputAction.CallbackContext context)
        {
            
        }

        public void EnterAimMode(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitAimMode(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}