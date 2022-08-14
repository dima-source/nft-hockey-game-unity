using System;
using System.Linq;
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
        
        public ButtonView[] buttons = new ButtonView[0];

        private UiButton[] _uiButtons;

        [Header("Input")] 
        [SerializeField]
        private bool input;
        
        private TextInformation _title;
        private TextInformation _message;
        private Transform _buttonsContainer;
        private InputNear _input;
        
        public Action onClose;

        protected override void Initialize()
        {
            _title = Utils.FindChild<TextInformation>(transform, "Title");
            _message = Utils.FindChild<TextInformation>(transform, "Message");
            Button background = Utils.FindChild<Button>(transform, "Background");
            background.onClick.RemoveAllListeners();
            background.onClick.AddListener(Close);
            _buttonsContainer = Utils.FindChild<Transform>(transform, "ButtonsContainer");
            foreach (Transform child in _buttonsContainer)
            {
                if (child.GetComponent<UiButton>() == null)
                {
                    throw new ApplicationException($"Invalid child '{child.name}'");
                }
            }

            _uiButtons = new UiButton[_buttonsContainer.childCount];
            for (int i = 0; i < _buttonsContainer.childCount; i++)
            {
                _uiButtons[i] = _buttonsContainer.GetChild(i).GetComponent<UiButton>();
            }
            
            _input = Utils.FindChild<InputNear>(transform, "InputNear");
        }
        
        protected override void OnUpdate()
        {
            _title.text = titleText;
            _message.text = messageText;
            UpdateButtons();
            _input.gameObject.SetActive(input);   
        }

        public void SetTitle(string value)
        {
            titleText = value;
        }

        public void OnButtonClick(int buttonIndex, Action action)
        {
            if (buttonIndex < 0 || buttonIndex >= buttons.Length)
            {
                throw new ApplicationException("ButtonIndex was out of range");
            }
            _uiButtons[buttonIndex].onClick = action;
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

        public double GetInputValue()
        {
            if (!input)
            {
                throw new ApplicationException("Input is disabled");
            }

            return _input.Value;
        }


        private void UpdateButtons()
        {
            if (buttons.Length > _uiButtons.Length)
            {
                buttons = buttons.Take(_uiButtons.Length).ToArray();
            }
            
            for (int i = 0; i < _uiButtons.Length; i++)
            {
                GameObject buttonObject = _uiButtons[i].gameObject;
                buttonObject.SetActive(i < buttons.Length);
                if (i < buttons.Length)
                {
                    SetButtonBackground(_uiButtons[i], buttons[i].type);
                    _uiButtons[i].text = buttons[i].text;
                }
            }
        }

        private void SetButtonBackground(UiButton button, ButtonType type)
        {
            button.material = type switch
            {
                ButtonType.Positive => TextInformation.BackgroundMaterial.AccentBackground1,
                ButtonType.Neutral => TextInformation.BackgroundMaterial.PrimaryBackground,
                ButtonType.Negative => TextInformation.BackgroundMaterial.AccentBackground2,
                _ => throw new ApplicationException("Unsupported type")
            };
        }

    }
}
