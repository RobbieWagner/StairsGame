using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RobbieWagnerGames.Player
{
    public interface IPlayer
    {
        void KillPlayer();
        int CurrentFlight();
    }
}
