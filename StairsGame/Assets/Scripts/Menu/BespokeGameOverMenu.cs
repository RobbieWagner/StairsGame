using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RobbieWagnerGames.Menu
{
    public class BespokeGameOverMenu : Menu
    {
        public override void SetupMenu(bool registerActionCollection = true)
        {
            foreach(TextMeshProUGUI textMeshPro in menuButtons.Select(b => b.GetComponentInChildren<TextMeshProUGUI>()))
            {
                if(textMeshPro != null)
                {
                    textMeshPro.enabled = true;
                }
            }
            
            base.SetupMenu(registerActionCollection);
        }
    }
}