using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class StairsManager : MonoBehaviour, IStairsManager
    {
        [SerializeField] private List<Stairs> activeStairs;
        [SerializeField] private List<GameObject> stairOptions;
        [SerializeField] private Transform staircaseParent;

        public static StairsManager Instance {get; private set;}
        private void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(gameObject); 
            else 
                Instance = this; 
        }

        public void AddStairs(Stairs stairs)
        {
            activeStairs.Add(stairs);
        }

        public void RemoveStairs(Stairs stairs, bool destroyStairs = false)
        {
            activeStairs.Remove(stairs);
            if(destroyStairs && stairs != null)
            {
                OnStairsDestroyed?.Invoke(stairs.flightNumber, activeStairs);
                Destroy(stairs.parent);
            }
        }
        public delegate void OnStairsDestroyedDelegate(int flight, List<Stairs> stairs);
        public event OnStairsDestroyedDelegate OnStairsDestroyed;

        public void InstantiateNewStairs()
        {
            int newFlightNumber = GetMaxActiveFlightValue() + 1;
            var stairPrefabs = stairOptions.Select(s => s.GetComponentInChildren<Stairs>()).Where(e => e.specs.ascendsRight == (newFlightNumber % 2 == 0));

            Stairs newStairs = null;
            if(stairPrefabs.Any())
            {
                newStairs = Instantiate(stairPrefabs.Select(s => s).ElementAt(Random.Range(0, stairPrefabs.Count())).parent, 
                                                staircaseParent).GetComponentInChildren<Stairs>();
                newStairs.SetFlightValue(newFlightNumber);
                Stairs prevFlight = GetTopFlight();
                newStairs.parent.transform.position = new Vector2(0, prevFlight.parent.transform.position.y + newStairs.specs.flightHeight - newStairs.specs.prevFlightOverlap); //TODO figure out x position placement
                AddStairs(newStairs);
            }
            RemoveUnneededFlights();
            if(newStairs != null)
                OnStairsAdded?.Invoke(newStairs);
        }
        public delegate void OnStairsAddedDelegate(Stairs newStairs);
        public event OnStairsAddedDelegate OnStairsAdded;

        public int GetMaxActiveFlightValue() => activeStairs.Max(s => s.GetFlightValue());
        public Stairs GetTopFlight() => activeStairs.Where(s => s.GetFlightValue() == GetMaxActiveFlightValue()).First();

        public void RemoveUnneededFlights()
        {
            List<Stairs> stairsToRemove = activeStairs.Where(s => s.flightNumber < PlayerInstance.Instance.CurrentFlight() - 6).ToList();
            foreach(Stairs stairs in stairsToRemove)
            {
                if(stairs != null) 
                    RemoveStairs(stairs, true);
            }
        }
    }
}