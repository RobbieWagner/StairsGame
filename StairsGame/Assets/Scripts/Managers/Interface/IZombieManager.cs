using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public interface IZombieManager
    {
        void Initialize();
        bool AddZombieToScene(Zombie zombie, Stairs stairs);
        bool RemoveZombieFromScene(Zombie zombie);
    }
}