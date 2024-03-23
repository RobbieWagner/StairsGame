using System;
using System.Collections;
using RobbieWagnerGames.ZombieStairs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.Menu
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [HideInInspector] public Menu parentMenu;
        [SerializeField] protected TextMeshProUGUI nameText;
        public const float DISABLED_TEXT_ALPHA = .4f;

        protected virtual void Awake()
        {
            nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, DISABLED_TEXT_ALPHA);
        }

        public virtual void NavigateTo() => nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 1);

        public virtual void NavigateAway() => nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, DISABLED_TEXT_ALPHA);

        public virtual IEnumerator SelectButton(Menu menu)
        {
            yield return null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(parentMenu != null) 
            {
                StartCoroutine(SelectButton(parentMenu));
                parentMenu?.DisableMenu();
                parentMenu?.InvokeOnSelectMenuItem();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => parentMenu?.ConsiderMenuButton(this);
        public void OnPointerExit(PointerEventData eventData) => parentMenu?.ConsiderMenuButton(null);
    }
}