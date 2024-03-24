using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RobbieWagnerGames.ZombieStairs
{
    public class GameUI : MonoBehaviour, IGameUI
    {
        private Coroutine currentScoreIncreaseCoroutine = null;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private int currentDisplayedScore;
        [SerializeField] private int scoreDisplayDigits = 17;
        private const int SCORE_CLIMB = 5;

        private void Awake()
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
        }

        public void UpdateScore(int score)
        {
            if(currentScoreIncreaseCoroutine == null)
                currentScoreIncreaseCoroutine = StartCoroutine(UpdateScoreCo());
        }

        private IEnumerator UpdateScoreCo()
        {
            while(currentDisplayedScore < GameManager.Instance.Score)
            {
                currentDisplayedScore += SCORE_CLIMB;
                DisplayCurrentScore();
                yield return null;
            }

            currentScoreIncreaseCoroutine = null;
        }

        private void DisplayCurrentScore()
        {
            string score = currentDisplayedScore.ToString();
            string displayScore = new string('0', scoreDisplayDigits - score.Length) + score;
            scoreText.text = displayScore;
        }
    }
}