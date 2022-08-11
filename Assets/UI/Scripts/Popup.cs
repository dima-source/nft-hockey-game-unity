using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class Popup : UiComponent
    {
        private enum ButtonType
        {
            Positive,
            Neutral,
            Negative
        }

        [Serializable]
        private class ButtonView
        {
            public ButtonType type;
            public string text;
        }
        
        
        [Header("Texts")]
        [SerializeField]
        private string titleText;
        [SerializeField]
        private string messageText;

        [SerializeField]
        private ButtonView[] buttons = new ButtonView[0];

        private UiButton[] _uiButtons;

        [Header("Input")] 
        [SerializeField]
        private bool input;
        
        private TextInformation _title;
        private TextInformation _message;
        private Transform _buttonsContainer;
        private InputNear _input;

        protected override void Initialize()
        {
            _title = Utils.FindChild<TextInformation>(transform, "Title");
            _message = Utils.FindChild<TextInformation>(transform, "Message");
            Button background = Utils.FindChild<Button>(transform, "Background");
            background.onClick.AddListener(OnCLose);
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

        public void AddButtonClickListener(int buttonIndex, UnityAction action)
        {
            if (buttonIndex < 0 || buttonIndex >= buttons.Length)
            {
                throw new ApplicationException("ButtonIndex was out of range");
            }
            _uiButtons[buttonIndex].AddOnClickListener(action);
        }
        
        public void SetMessage(string value)
        {
            messageText = value;
        }

        private void OnCLose()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
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
                GameObject buttonObject = _buttonsContainer.GetChild(i).gameObject;
                buttonObject.SetActive(i < buttons.Length);
                if (i < buttons.Length)
                {
                    UiButton button = buttonObject.GetComponent<UiButton>();
                    SetButtonBackground(button, buttons[i].type);
                    button.text = buttons[i].text;
                }
            }
        }

        private void SetButtonBackground(UiButton button, ButtonType type)
        {
            button.material = type switch
            {
                ButtonType.Positive => TextInformation.BackgroundMaterial.AccentBackground1,
                ButtonType.Neutral => TextInformation.BackgroundMaterial.SecondaryBackground,
                ButtonType.Negative => TextInformation.BackgroundMaterial.AccentBackground2,
                _ => throw new ApplicationException("Unsupported type")
            };
        }

    }
}
