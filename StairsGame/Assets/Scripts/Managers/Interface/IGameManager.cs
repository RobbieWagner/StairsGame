using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.Managers
{
    public enum GameState
    {
        None = 0,
        Playing = 1,
        Dead = 2
    }

    public interface IGameManager 
    {
        public GameState CurrentGameState();
        public bool ChangeGameState(GameState newState);
    }
}