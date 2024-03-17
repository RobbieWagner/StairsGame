using RobbieWagnerGames.Player;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public interface IStairs
    {
        void UpdateColliderForEntity(IStairsActor agent, bool onBackground);
        bool isFlightValueEven();
        bool isStairsForeground();
    }
}