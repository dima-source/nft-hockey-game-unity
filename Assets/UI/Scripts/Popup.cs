using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class Popup : UiComponent
    {
        public enum ButtonType
        {
            Positive,
            Neutral,
            Negative
        }

        [Serializable]
        public class ButtonView
        {
            public ButtonType type;
            public string text;

            public ButtonView() { }
            
            public ButtonView(ButtonType type, string text)
            {
                this.type = type;
                this.text = text;
            }
        }
        
        
        [Header("Texts")]
        [SerializeField]
        private string titleText;
        [SerializeField]
        private string messageText;
        
        public ButtonView[] buttons;
        private Button[] _sceneButtons;
        
        private TextMeshProUGUI _title;
        private TextMeshProUGUI _message;
        private Transform _buttonsContainer;
        public Action onClose;

        protected override void Initialize()
        {
            _title = Utils.FindChild<TextMeshProUGUI>(transform, "TitleText");
            _message = Utils.FindChild<TextMeshProUGUI>(transform, "MessageText");
            Button background = Utils.FindChild<Button>(transform, "Background");
            background.onClick.RemoveAllListeners();
            background.onClick.AddListener(Close);
            _buttonsContainer = Utils.FindChild<Transform>(transform, "ButtonsContainer");
            foreach (Transform child in _buttonsContainer)
            {
                if (child.GetComponent<Button>() == null)
                {
                    throw new ApplicationException($"Invalid child '{child.name}'");
                }
            }

            _sceneButtons = new Button[_buttonsContainer.childCount];
            for (int i = 0; i < _buttonsContainer.childCount; i++)
            {
                _sceneButtons[i] = _buttonsContainer.GetChild(i).GetComponent<Button>();
            }
        }
        
        protected override void OnUpdate()
        {
            _title.text = titleText;
            _message.text = messageText;
            UpdateButtons();
        }

        public void SetTitle(string value)
        {
            titleText = value;
        }

        public void OnButtonClick(int buttonIndex, UnityAction action)
        {
            if (buttonIndex < 0 || buttonIndex >= buttons.Length)
            {
                throw new ApplicationException("ButtonIndex was out of range");
            }
            _sceneButtons[buttonIndex].onClick.RemoveAllListeners();
            _sceneButtons[buttonIndex].onClick.AddListener(() =>
            {
                AudioController.LoadClip(Configurations.DefaultButtonSoundPath);
                AudioController.source.Play();
                action?.Invoke();
            });
        }
        
        public void SetMessage(string value)
        {
            messageText = value;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            onClose();
            gameObject.SetActive(false);
        }

        private void UpdateButtons()
        {
            if (buttons.Length > _sceneButtons.Length)
            {
                buttons = buttons.Take(_sceneButtons.Length).ToArray();
            }
            
            for (int i = 0; i < _sceneButtons.Length; i++)
            {
                GameObject buttonObject = _sceneButtons[i].gameObject;
                buttonObject.SetActive(i < buttons.Length);
                if (i < buttons.Length)
                {
                    SetButtonBackground(_sceneButtons[i], buttons[i].type);
                    TextMeshProUGUI textMesh = _sceneButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    textMesh.text = buttons[i].text;
                }
            }
        }

        private static void SetButtonBackground(Button button, ButtonType type)
        {
            string path = type switch
            {
                ButtonType.Positive => Configurations.MaterialsFolderPath + "AccentBackground1",
                ButtonType.Neutral => Configurations.MaterialsFolderPath + "PrimaryBackground",
                ButtonType.Negative => Configurations.MaterialsFolderPath + "AccentBackground2",
                _ => throw new ApplicationException("Unsupported type")
            }; 
            
            Image image = button.GetComponentInChildren<Image>();
            image.material = Utils.LoadResource<Material>(path);
        }

    }
}
