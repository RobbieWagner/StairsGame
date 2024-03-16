using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class Zombie : MonoBehaviour, IZombie
    {
        [SerializeField] protected int maxHealth;
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

        protected virtual void Awake()
        {
            health = maxHealth;
        }

        public void AttackPlayer(IPlayer player)
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

        public void ChasePlayer(IPlayer player)
        {
            throw new System.NotImplementedException();
        }
    }
}