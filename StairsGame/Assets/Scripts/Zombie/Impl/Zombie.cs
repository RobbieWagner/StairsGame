using System.Collections;
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class Zombie : MonoBehaviour, IZombie, IStairsActor
    {
        [SerializeField] protected int maxHealth;
        [SerializeField] private Collider2D zombieCollider;
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

        protected virtual void Awake()
        {
            health = maxHealth;
        }

        public void AttackPlayer(IStairsActor player)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public void ChasePlayer(IStairsActor player)
        {
            throw new System.NotImplementedException();
        }

        public int CurrentFlight()
        {
            throw new System.NotImplementedException();
        }

        public bool CanMoveToBackground()
        {
            throw new System.NotImplementedException();
        }

        public bool CanMoveToForeground()
        {
            throw new System.NotImplementedException();
        }

        public int CurrentFloor()
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentStairs(Stairs stairs)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator MoveToBackground()
        {
            yield return null;
            throw new System.NotImplementedException();
        }

        public IEnumerator MoveToForeground()
        {
            yield return null;
            throw new System.NotImplementedException();
        }

        public Collider2D GetCollider() => zombieCollider;
    }
}