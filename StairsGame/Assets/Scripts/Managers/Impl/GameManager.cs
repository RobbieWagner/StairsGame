using System.Collections;
using RobbieWagnerGames.Managers;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public static string persistentDataPath;
        private GameState currentGameState = GameState.None;
        public GameState CurrentGameState() => currentGameState;

        private void Awake()
        {
            persistentDataPath = Application.persistentDataPath;
            StartCoroutine(DelayStartCo());
            ChangeGameState(GameState.Playing);
        }

        private bool StartGame()
        {
            PlayerInstance.Instance.Initialize();
            return true;
        }

        public bool ChangeGameState(GameState newState)
        {
            if(currentGameState == newState)
                return false;

            switch(newState)
            {
                case GameState.None:
                break;
                case GameState.Playing:
                if(!StartGame())
                    return false;
                break;
                case GameState.Dead:
                if(PlayerInstance.Instance.KillPlayer())
                    return false;
                break;
                default:
                break;
            }

            currentGameState = newState;
            return true;
        }

        public IEnumerator DelayStartCo()
        {
            yield return null;
            PlayerInstance.Instance.Initialize();
        }
    }
}