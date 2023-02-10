using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class PopupBread : UiComponent
    {
        public TextMeshProUGUI _title;
        public TextMeshProUGUI _message;
        public string Title;
        public string Message; 
        private Button GoBack;
        private Button Accept;
        
        protected override void Initialize()
        {
            _title = UiUtils.FindChild<TextMeshProUGUI>(transform, "Title");
            _message = UiUtils.FindChild<TextMeshProUGUI>(transform, "Message");
            GoBack = UiUtils.FindChild<Button>(transform, "GoBack");
            GoBack.onClick.AddListener(() => Close());
            Accept = UiUtils.FindChild<Button>(transform, "Accept");
            Accept.onClick.AddListener(() => Close());
        }

        protected override void OnUpdate()
        {
            SetData(Title, Message);
        }

        public void SetData(string Title, string Message)
        {
            _title.text = Title;
            _message.text = Message;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}