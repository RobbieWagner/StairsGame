using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class ZombieManager : MonoBehaviour, IZombieManager
    {
        private HashSet<Zombie> zombiesInScene;
        [SerializeField] private Transform zombieParent;
        [SerializeField] private List<Zombie> zombieOptions;
        public bool canSpawnZombies = true;

        public static ZombieManager Instance {get; private set;}
        private void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(gameObject); 
            else 
                Instance = this; 

            zombiesInScene = new HashSet<Zombie>();
            StairsManager.Instance.OnStairsDestroyed += DestroyUnsupportedZombies;
            StairsManager.Instance.OnStairsAdded += SpawnZombie;
        }

        private void DestroyUnsupportedZombies(int flight, List<Stairs> stairs)
        {

            var activeFlights = stairs.Select(s => s.flightNumber);
            foreach(int f in activeFlights) Debug.Log(f);
            List<Zombie> zombies = zombiesInScene.Select(z => z).ToList();

            foreach(Zombie zombie in zombies)
            {
                if(!activeFlights.Contains(zombie.CurrentFlight()) && !zombie.canIgnoreStairDestruction)
                    RemoveZombieFromScene(zombie);
            }
        }

        public bool AddZombieToScene(Zombie zombie, Stairs stairs)
        {
            Zombie newZombie = Instantiate(zombie, zombieParent);
            if(newZombie != null)
                newZombie.transform.position = stairs.spawnPosition.position;

            newZombie.Initialize(stairs);

            return zombiesInScene.Add(newZombie);
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void SpawnZombie(Stairs stairsForZombie)
        {
            if(canSpawnZombies)
                AddZombieToScene(zombieOptions[UnityEngine.Random.Range(0, zombieOptions.Count)], stairsForZombie);
        }

        public bool RemoveZombieFromScene(Zombie zombie)
        {
            zombiesInScene.Remove(zombie);
            if(zombie != null) 
            {
                Destroy(zombie.gameObject);
                return true;
            }
            return false;
        }
    }
}