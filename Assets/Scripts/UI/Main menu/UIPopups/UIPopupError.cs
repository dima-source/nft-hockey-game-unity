using TMPro;
using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupError : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;

        public void SetTitle(string title)
        {
            titleText.text = title;
        }
        
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