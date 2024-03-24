using System.Collections;
using System.Net;
using RobbieWagnerGames.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using RobbieWagnerGames.Menu;

namespace RobbieWagnerGames.ZombieStairs
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Header("Score")]
        public static int newFloorReached = 100;

        [Header("Pausing")]
        public bool canPause = false;
        public bool paused = false;
 
        public static string persistentDataPath;
        private GameState currentGameState = GameState.None;
        private MainGameControls gameControls;
        public GameState CurrentGameState() => currentGameState;
        
        [Header("Game Over")]
        [SerializeField] private Canvas gameOverCanvas;
        [SerializeField] private Image gameOverBackground;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private BespokeGameOverMenu gameOverMenu;

        private int score = 0;
        public int Score
        {
            get { return score; }
            set 
            {
                if(score == value) 
                    return;
                
                score = value;
                if(score < 0)
                    score = 0;

                OnScoreChanged?.Invoke(score);
            }
        }
        public delegate void OnScoreChangedDelegate(int newScore);
        public event OnScoreChangedDelegate OnScoreChanged;

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
                if(!PlayerInstance.Instance.KillPlayer())
                    return false;
                StartCoroutine(GameOver());
                break;

                default:
                break;
            }

            currentGameState = newState;
            return true;
        }

        public IEnumerator GameOver()
        {
            ResumeGame();
            canPause = false;
            gameOverText.color = Color.clear;
            gameOverBackground.color = Color.clear;
            
            gameOverCanvas.enabled = true;
            yield return gameOverBackground.DOColor(Color.black, 1f).SetEase(Ease.InCubic).WaitForCompletion();
            yield return gameOverText.DOColor(Color.red, 2.5f).SetEase(Ease.InCubic).WaitForCompletion();
            gameOverMenu.SetupMenu();
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