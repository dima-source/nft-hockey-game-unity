using UI.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Main_menu.UIPopups
{
    public abstract class UIPopup : UiComponent
    {
        [FormerlySerializedAs("mainMenuView")] [SerializeField] protected MainMenu mainMenu;

        public void Show()
        {
            mainMenu.ShowPopup(transform);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}