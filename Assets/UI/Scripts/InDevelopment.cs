using System;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    [ExecuteInEditMode]
    public class InDevelopment : MonoBehaviour
    {
        [SerializeField]
        private string titleText;
        [SerializeField]
        private string messageText;
        [SerializeField]
        private string buttonText;

        private TextMeshProUGUI _title;
        private TextMeshProUGUI _message;
        [HideInInspector]
        public UiButton button;
        
        private void Awake()
        {
            Initialize();
            UpdateTexts();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Initialize();
        }
#endif
        
        private void Update()
        {
            UpdateTexts();
        }

        private void Initialize()
        {
            Transform container = Utils.FindChild<Transform>(transform, "Container");
            _title = Utils.FindChild<TextMeshProUGUI>(container, "TitleText");
            _message = Utils.FindChild<TextMeshProUGUI>(container, "MessageText");
            Transform buttonContainer = Utils.FindChild<Transform>(container, "ButtonContainer");
            button = Utils.FindChild<UiButton>(buttonContainer, "Button");
        }

        private void UpdateTexts()
        {
            _title.text = titleText;
            _message.text = messageText;
            button.text = buttonText;
        }

    }
}
