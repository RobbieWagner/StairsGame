using System.Collections;
using RobbieWagnerGames.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RobbieWagnerGames.ZombieStairs
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Header("Pausing")]
        public bool canPause = false;
        public bool paused = false; 
        public static string persistentDataPath;
        private GameState currentGameState = GameState.None;
        private MainGameControls gameControls;
        public GameState CurrentGameState() => currentGameState;

        public static GameManager Instance {get; private set;}

        private void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(gameObject); 
            else 
                Instance = this; 

            persistentDataPath = Application.persistentDataPath;
            StartCoroutine(DelayStartCo());
            ChangeGameState(GameState.Playing);

            gameControls = new MainGameControls();
            gameControls.Pausing.Pause.performed += TogglePause;
            
            //TODO: handle outside of game manager
            canPause = true;
            gameControls.Enable();
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

         public bool PauseGame()
        {
            if(canPause)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
                paused = true;
                IInputManager.Instance.DisableActions();
                OnPauseGame?.Invoke();
                return true;
            }
            return false;
        }
        public delegate void OnPauseGameDelegate();
        public event OnPauseGameDelegate OnPauseGame;

        public void ResumeGame()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            IInputManager.Instance.ReenableActions();
            OnResumeGame?.Invoke();
            paused = false;
        }
        public delegate void OnResumeGameDelegate();
        public event OnResumeGameDelegate OnResumeGame;

        private void TogglePause(InputAction.CallbackContext context)
        {
            Debug.Log("pause toggled");
            if(!paused)
                PauseGame();
            else
                ResumeGame();
        }
    }
}