using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames.ZombieStairs;

namespace RobbieWagnerGames.Menu
{
    public class ResumeGameButton : MenuButton
    {
        public override IEnumerator SelectButton(Menu menu)
        {
            yield return new WaitForSecondsRealtime(.01f);
            GameManager.Instance.ResumeGame();
        }
    }
}

