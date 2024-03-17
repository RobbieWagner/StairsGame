using System.Collections.Generic;
using JetBrains.Annotations;
using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class Stairs : MonoBehaviour, IStairs
    {
        [field: SerializeField] public int flightNumber {get; private set;}
        public int floorNumber => flightNumber/2;
        public static bool EVEN_FLIGHT_MEANS_FOREGROUND = true;

        public Collider2D stairsCollider;
        private HashSet<IStairsActor> stairsActors;

        public bool isFlightValueEven() => flightNumber % 2 == 0;

        private void Awake()
        {
            stairsActors = new HashSet<IStairsActor>();
            RegisterEntity(PlayerInstance.Instance);
            UpdateColliderForEntity(PlayerInstance.Instance);
        }

        public void RegisterEntity(IStairsActor stairsActor)
        {
            stairsActors.Add(stairsActor);
            stairsActor.OnMoveBackwardForward += UpdateColliderForEntity;
        }

        public void DeregisterEntity(IStairsActor stairsActor)
        {
            if(stairsActors.Contains(stairsActor))
                stairsActor.OnMoveBackwardForward -= UpdateColliderForEntity;
            stairsActors.Remove(stairsActor);
        }

        public void UpdateColliderForEntity(IStairsActor agent, bool toForeground = true)
        {
            if(toForeground == isStairsForeground())
            {
                Physics2D.IgnoreCollision(agent.GetCollider(), stairsCollider, false);
                Debug.Log($"flight {flightNumber} not ignoring player");
            }
            else
                Physics2D.IgnoreCollision(agent.GetCollider(), stairsCollider, true);
                Debug.Log($"flight {flightNumber} ignoring player");
            
        }

        //Return true if stairs are foreground (flight is even and foreground flights are even or both are odd)
        public bool isStairsForeground() => EVEN_FLIGHT_MEANS_FOREGROUND == isFlightValueEven();
    }
}
