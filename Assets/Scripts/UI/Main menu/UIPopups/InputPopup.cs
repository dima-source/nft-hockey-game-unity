using UI.Scripts;
using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class InputPopup: UiComponent
    {
        private Loading _spinner;
        private Transform _textArea;
        private Transform _footer;
        private Transform _inputArea;
        
        protected override void Initialize()
        {
            _spinner = UiUtils.FindChild<Loading>(transform, "Loading");
            _textArea = UiUtils.FindChild<Transform>(transform, "TextArea");
            _footer = UiUtils.FindChild<Transform>(transform, "Footer");
            _inputArea = UiUtils.FindChild<Transform>(transform, "InputArea");
        }

        public void ShowSpinner()
        {
            SetActiveLogicFields(false);
            _spinner.gameObject.SetActive(true);
        }
        
        public void HideSpinner()
        {
            SetActiveLogicFields(true);
            _spinner.gameObject.SetActive(false);
        }

        private void SetActiveLogicFields(bool isActive)
        {
            _textArea.gameObject.SetActive(isActive);
            _footer.gameObject.SetActive(isActive);
            _inputArea.gameObject.SetActive(isActive);
        }
    }
}