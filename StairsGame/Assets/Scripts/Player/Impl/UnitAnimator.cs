using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.Player
{
    public enum UnitAnimationState
    {
        // movement
        Idle = 0,
        IdleForward = 1,
        IdleLeft = 2,
        IdleRight = 3,
        
        WalkForward = 4,
        WalkBack = 5,
        WalkLeft = 6,
        WalkRight = 7,

        RunForward = 8,
        RunBack = 9,
        RunLeft = 10,
        RunRight = 11
    }

    public class UnitAnimator : MonoBehaviour
    {

        [SerializeField] public Animator animator;

        [SerializeField] private List<UnitAnimationState> states;
        private UnitAnimationState currentState;

        protected virtual void Awake()
        {
            OnAnimationStateChange += StartAnimation;
            ChangeAnimationState(UnitAnimationState.Idle);
        }

        public void ChangeAnimationState(UnitAnimationState state)
        {
            if(state != currentState && states.Contains(state)) 
            {
                currentState = state;
                
                OnAnimationStateChange(state);
            }
            else if(state != currentState)
            {
                Debug.Log("Animation Clip Not Set Up For Unit");
            }
        }

        public delegate void OnAnimationStateChangeDelegate(UnitAnimationState state);
        public event OnAnimationStateChangeDelegate OnAnimationStateChange;

        public UnitAnimationState GetAnimationState()
        {
            return currentState;
        }

        protected void StartAnimation(UnitAnimationState state)
        {
            animator.Play(state.ToString());
        }
    }
}