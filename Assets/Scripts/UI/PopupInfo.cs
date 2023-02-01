using TMPro;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class PopupInfo : UiComponent
    {
        private Button _closeButton;
        private TMP_Text _titleText;
        private TMP_Text _infoText;
        
        protected override void Initialize()
        {
            _titleText = UiUtils.FindChild<TMP_Text>(transform, "TitleText");
            _infoText = UiUtils.FindChild<TMP_Text>(transform, "InfoText");
            _closeButton = UiUtils.FindChild<Button>(transform, "GoBack");
            _closeButton.onClick.AddListener(ClosePopup);
        }

        public void SetTitle(string title)
        {
            _titleText.text = title;
        }

        public void SetInfo(string info)
        {
            _infoText.text = info;
        }

        private void ClosePopup()
        {
            Destroy(gameObject);
        }
    }
}