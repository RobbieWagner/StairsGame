using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames;
using UnityEngine.InputSystem;
using FMODUnity;

namespace RobbieWagnerGames.Player
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] Rigidbody2D body;
        [SerializeField] private LayerMask groundMask; 
        private Vector2 lastFramePos;
        [SerializeField] UnitAnimator unitAnimator;
        private MovementControls playerMovementControls;
        private const float GRAVITY = 9.8f;

        private float movementInput;
        private bool moving;
        float moveLimiter = 0.7f;

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


        private void Awake() 
        {
            canMove = true;
            movementSpeed = defaultWalkSpeed;
            running = false;
            hasRecentlyMoved = false;

            //unitAnimator = GetComponent<UnitAnimator>();

            currentRunSpeed = defaultRunSpeed;
            currentWalkSpeed = defaultWalkSpeed;

            playerMovementControls = new MovementControls();
            playerMovementControls.Enable();

            playerMovementControls.Movement.Move.performed += OnMove;
            playerMovementControls.Movement.Move.canceled += OnStopMoving;
            playerMovementControls.Movement.Run.performed += OnStartRun;
            playerMovementControls.Movement.Run.canceled += OnStopRun;

            playerMovementControls.Movement.Interact.performed += OnInteract;
        }

        private void FixedUpdate() => body.velocity = new Vector2(movementInput * movementSpeed, isGrounded ? 0 : body.velocity.y - GRAVITY * Time.deltaTime);

        private void LateUpdate()
        {
            UpdateGroundCheck();

            // if (characterController.enabled) 
            //     characterController.Move(movementVector * currentWalkSpeed * Time.deltaTime);

            lastFramePos = transform.position;

            // if (moving)
            //     PlayMovementSounds();
        }

        private void UpdateGroundCheck()
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2) transform.position + new Vector2(0, -.4f), Vector2.down, -.01f, groundMask);
            isGrounded = hit.collider != null;
            Debug.Log($"{isGrounded}");

            //TODO: Calculate slope for physics updating

            // if (hit.collider != null)
            // {
            //     GroundInfo groundInfo = hit.collider.gameObject.GetComponent<GroundInfo>();
            //     if (groundInfo != null)
            //         CurrentGroundType = groundInfo.groundType;  
            //     else
            //         CurrentGroundType = GroundType.None;
            // }
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
            if(movementInput > 0)unitAnimator.ChangeAnimationState(UnitAnimationState.IdleRight);
            else if(movementInput < 0)unitAnimator.ChangeAnimationState(UnitAnimationState.IdleLeft);
            else if(movementInput > 0) unitAnimator.ChangeAnimationState(UnitAnimationState.IdleForward);
            else unitAnimator.ChangeAnimationState(UnitAnimationState.Idle);
            moving = false;
            //movementSounds?.ToggleMovementSounds(false);

            movementInput = 0;
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

        public void ChangePlayerSpeed(float newWalkSpeed, float newRunSpeed)
        {
            currentWalkSpeed = newWalkSpeed;
            currentRunSpeed = newRunSpeed;

            if(running) movementSpeed = currentRunSpeed;
            else movementSpeed = currentWalkSpeed;
        }

        public void ResetSpeeds()
        {
            currentRunSpeed = defaultRunSpeed;
            currentWalkSpeed = defaultWalkSpeed;

            if(running) movementSpeed = currentRunSpeed;
            else movementSpeed = currentWalkSpeed;
        }
        private void OnInteract(InputAction.CallbackContext context)
        {
        }
    }
}