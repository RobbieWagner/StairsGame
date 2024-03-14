using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.Managers
{
    public enum GameState
    {
        None = 0
    }

    public interface IGameManager 
    {
        public GameState CurrentGameState();
    }
}