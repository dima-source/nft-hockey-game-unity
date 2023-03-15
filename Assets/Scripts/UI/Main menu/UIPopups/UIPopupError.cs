using TMPro;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupError : UiComponent
    {
        [SerializeField] private TextMeshProUGUI titleText;
        private Button _confirmButton;

        protected override void Initialize()
        {
            _confirmButton = UiUtils.FindChild<Button>(transform, "Confirm");
        }
        
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
            _confirmButton.onClick.AddListener(action);
            var buttonTitleText = UiUtils.FindChild<TextMeshProUGUI>(_confirmButton.transform, "Text");
            buttonTitleText.text = buttonTitle;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}