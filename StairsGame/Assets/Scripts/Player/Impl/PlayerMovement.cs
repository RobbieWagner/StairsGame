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
        [SerializeField] UnitAnimator unitAnimator;
        private MovementControls playerMovementControls;

        private Vector2 movementVector;
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


        private void Awake() 
        {
            canMove = true;
            movementSpeed = defaultWalkSpeed;
            running = false;
            hasRecentlyMoved = false;

            unitAnimator = GetComponent<UnitAnimator>();

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

        private void FixedUpdate() => body.velocity = new Vector2(movementVector.x * movementSpeed, movementVector.y * movementSpeed);

        private void OnMove(InputAction.CallbackContext context)
        {
            if(canMove)
            {
                Vector2 input = context.ReadValue<Vector2>();

                if(movementVector.x != input.x && input.x != 0f)
                {
                    if(input.x > 0) 
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
                else if(input.x == 0 && movementVector.y != input.y && input.y != 0f)
                {
                    if(input.y > 0) 
                    {
                        if(running)
                            unitAnimator.ChangeAnimationState(UnitAnimationState.RunForward);
                        else
                            unitAnimator.ChangeAnimationState(UnitAnimationState.WalkForward);
                    }
                    else  
                    {
                        if(running)
                            unitAnimator.ChangeAnimationState(UnitAnimationState.RunBack);
                        else
                            unitAnimator.ChangeAnimationState(UnitAnimationState.WalkBack);
                    }
                    moving = true;
                }
                
                movementVector.x = input.x;
                movementVector.y = input.y;
            }
        }

        private void OnStopMoving(InputAction.CallbackContext context)
        {
            if(movementVector.x > 0)unitAnimator.ChangeAnimationState(UnitAnimationState.IdleRight);
            else if(movementVector.x < 0)unitAnimator.ChangeAnimationState(UnitAnimationState.IdleLeft);
            else if(movementVector.y > 0) unitAnimator.ChangeAnimationState(UnitAnimationState.IdleForward);
            else unitAnimator.ChangeAnimationState(UnitAnimationState.Idle);
            moving = false;
            //movementSounds?.ToggleMovementSounds(false);

            movementVector = Vector2.zero;
        }

        public void OnStartRun(InputAction.CallbackContext context)
        {
            running = true;
            //movementSounds?.ToggleRun(true);
            movementSpeed = currentRunSpeed;

            if(moving)
            {
                if(Mathf.Abs(movementVector.x) > Mathf.Abs(movementVector.y))
                {
                    if(movementVector.x > 0) 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.RunRight);
                    else 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.RunLeft);
                }
                else
                {
                    if(movementVector.y > 0)
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
                if(Mathf.Abs(movementVector.x) > Mathf.Abs(movementVector.y))
                {
                    if(movementVector.x > 0) 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.WalkRight);
                    else 
                        unitAnimator.ChangeAnimationState(UnitAnimationState.WalkLeft);
                }
                else
                {
                    if(movementVector.y > 0)
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