using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class UiButton : UiComponent
    {
        public enum ButtonType
        {
            Positive,
            Neutral,
            Negative
        }

        private ButtonType buttonType = ButtonType.Neutral;
        private TextMeshProUGUI text;
        private Button _button;
        private Image _stroke;
        private Image _background;
        private float _widthStroke = 5;
        
        public Action ONClick { get; set; }
         
        private const string DefaultSound = "click v1";

        protected override void Initialize()
        {
            _stroke = GetComponent<Image>();
            _background = UiUtils.FindChild<Image>(transform, "MainArea");
            _button = GetComponent<Button>();
            text =  UiUtils.FindChild<TextMeshProUGUI>(transform, "Text");
            var strokeRect = GetComponent<RectTransform>();
            var backgroundRect = UiUtils.FindChild<RectTransform>(transform, "MainArea");
            var sizeDeltaStroke = strokeRect.sizeDelta;
            var sizeDeltaBackground = backgroundRect.sizeDelta;
            _widthStroke = sizeDeltaStroke.x / 40;
            
            backgroundRect.sizeDelta = new Vector2(sizeDeltaStroke.x - _widthStroke,
               sizeDeltaStroke.y - _widthStroke);

            backgroundRect.anchoredPosition = new Vector2(0, 0);
            ONClick = () => { };
            
            //SetButtonType(buttonType, text.text);
        }
        
        public void SetButtonType(ButtonType type, string textString)
        {
            buttonType = type;
            text.text = textString;
            
            SetMaterials();
            SetSprites();
        }

        private void SetMaterials()
        {
            string pathBackground = ButtonUtils.BackgroundMatPath[buttonType];
            _background.material = UiUtils.LoadResource<Material>(pathBackground);

            string pathStroke = ButtonUtils.StrokeMatPath[buttonType];
            _background.material = UiUtils.LoadResource<Material>(pathStroke);
        }

        private void SetSprites()
        {
            string pathBackground = ButtonUtils.BackgroundSpritePath[buttonType];
            _background.sprite = UiUtils.LoadResource<Sprite>(pathBackground);

            string pathStroke = ButtonUtils.StrokeSpritePath[buttonType];
            _background.sprite= UiUtils.LoadResource<Sprite>(pathStroke);
        }
        
        public void AddListener(UnityAction action, string sound = DefaultSound)
        {
            AudioController.LoadClip(Configurations.MusicFolderPath + sound);
            AudioController.source.Play();
            _button.onClick.AddListener(action);
        }

        public void RemoveAllListeners()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}