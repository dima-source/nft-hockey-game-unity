using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Near.Models.Tokens;
using TMPro;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Popup : UiComponent
    {
        [Serializable]
        public class ButtonView
        {
            public UiButton.ButtonType type;
            public string text;

            public ButtonView() { }
            
            public ButtonView(UiButton.ButtonType type, string text)
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
        
        public TextMeshProUGUI _bidCount;
        private TextMeshProUGUI _title;
        private TextMeshProUGUI _message;
        private Transform _buttonsContainer;
        public UnityAction onClose;
        public Transform bidContainer;
        public Button _bidInfo;
        public PopupInfo _popupInfo;
        public Transform _inputField;
        private static readonly string Path = Configurations.PrefabsFolderPath + "Popups/InfoPopup";
        protected override void Initialize()
        { 
            //_inputField = UiUtils.FindChild<InputField>(transform, "InputField");
            _bidCount = UiUtils.FindChild<TextMeshProUGUI>(transform, "BidCount");
            _bidInfo = UiUtils.FindChild<Button>(transform, "BidInfo");
            _title = UiUtils.FindChild<TextMeshProUGUI>(transform, "TitleText");
            _bidCount = UiUtils.FindChild<TextMeshProUGUI>(transform, "BidCount");
            _message = UiUtils.FindChild<TextMeshProUGUI>(transform, "MessageText");
            Button background = UiUtils.FindChild<Button>(transform, "Background");
            background.onClick.RemoveAllListeners();
            background.onClick.AddListener(Close);
            bidContainer = UiUtils.FindChild<Transform>(transform, "BidContainer");
            _buttonsContainer = UiUtils.FindChild<Transform>(transform, "ButtonsContainer");
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
        
        public void ShowBidStory(string betInfo)
        {
            if (_popupInfo != null)
            {
                Destroy(_popupInfo.gameObject);
            }
            
            GameObject prefab = UiUtils.LoadResource<GameObject>(Path);
            _popupInfo = Instantiate(prefab, transform).GetComponent<PopupInfo>();
            _popupInfo.SetTitle("Bid story");
            _popupInfo.SetInfo(betInfo);
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
        public void DeleteMessageSlot()
        {
            _message.gameObject.SetActive(false);
        }

        public void ShowBidConainer(bool value)
        {
            if (value)
            {
                bidContainer.gameObject.SetActive(true);
            }
            else
            {
                bidContainer.gameObject.SetActive(false);
            }
        }

        public void AddAdditional(Transform go)
        {
            Transform additional = UiUtils.FindChild<Transform>(transform, "Additional");
            /*
            additional.GetComponent<Image>().color = new Color()
            {
                r = (float)56 / 255,
                g = (float)56 / 255,
                b = (float)56 / 255,
                a = 1
            };
            */
            int position = additional.childCount - 1;
            go.SetParent(additional, false);
            go.SetSiblingIndex(Math.Max(0, position));
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
            onClose?.Invoke();
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

        private static void SetButtonBackground(Button button, UiButton.ButtonType type)
        {
            /*
            string path = type switch
            {
                UiButton.ButtonType.Positive => Configurations.MaterialsFolderPath + "AccentBackgroundCold",
                UiButton.ButtonType.Neutral => Configurations.MaterialsFolderPath + "PrimaryBackground",
                UiButton.ButtonType.Negative => Configurations.MaterialsFolderPath + "AccentBackground2",
                _ => throw new ApplicationException("Unsupported type")
            }; 
            
            Image image = button.GetComponentInChildren<Image>();
            */
            
            //----------------
            //image.material = Utils.LoadResource<Material>(path);
        }
    }
}
