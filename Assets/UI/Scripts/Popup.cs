using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    public class Popup : UiComponent
    {
        [SerializeField]
        private string titleText;
        [SerializeField]
        private string messageText;
        
        private TextMeshProUGUI _title;
        private TextMeshProUGUI _message;

        protected override void Initialize()
        {
            Transform container = Utils.FindChild<Transform>(transform, "Container");
            
            //_title = Utils.FindChild<TextMeshProUGUI>(container, "TitleText");
            //_message = Utils.FindChild<TextMeshProUGUI>(container, "MessageText");
        }
        
        protected override void OnUpdate()
        {
            //_title.text = titleText;
            //_message.text = messageText;   
        }

    }
}
