using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public interface IStairsManager
    {
        void InstantiateNewStairs();
        void AddStairs(Stairs stairs);
        void RemoveStairs(Stairs stairs, bool destroyStairs = false);
        int GetMaxActiveFlightValue();
        void RemoveUnneededFlights();
    }
}

