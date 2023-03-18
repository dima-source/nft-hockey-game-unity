using TMPro;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupError : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button confirmButton;
        
        public void SetTitle(string title)
        {
            titleText.text = title;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void BidButton(UnityAction action, string buttonTitle)
        {
            confirmButton.onClick.AddListener(action);
            var buttonTitleText = UiUtils.FindChild<TextMeshProUGUI>(confirmButton.transform, "Text");
            buttonTitleText.text = buttonTitle;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}