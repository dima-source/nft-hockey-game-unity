using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupNotEnoughBalance : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}