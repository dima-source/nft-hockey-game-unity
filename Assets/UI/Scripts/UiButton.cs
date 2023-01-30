using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class UiButton : UiComponent, IPointerClickHandler
    {
        public enum ButtonType
        {
            Positive,
            Neutral,
            Negative
        }

        [SerializeField] private ButtonType buttonType = ButtonType.Neutral;
        private Image _stroke;
        private Image _background;
        private float _widthStroke = 5;
        private TextMeshProUGUI _text;
        public Action ONClick { get; set; }
         
        private const string DefaultSound = "click v1";

        protected override void Initialize()
        {
            _stroke = GetComponent<Image>();
            _background = Utils.FindChild<Image>(transform, "MainArea");
            
            var strokeRect = GetComponent<RectTransform>();
            var backgroundRect = Utils.FindChild<RectTransform>(transform, "MainArea");
            var sizeDelta = strokeRect.sizeDelta;
            backgroundRect.sizeDelta = new Vector2(sizeDelta.x - _widthStroke,
                sizeDelta.y - _widthStroke);
            
            ONClick = () => { };
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.LoadClip(Configurations.MusicFolderPath + DefaultSound);
            AudioController.source.Play();
            ONClick?.Invoke();
        }
        
        public void SetButtonType(Enum buttonType, string text)
        {
            _text.text = text;
            SetMaterial(buttonType);
        }

        public void SetMaterial(Enum buttonType)
        {
            
            string pathBackground = buttonType switch
            {
                ButtonType.Positive => Configurations.MaterialsFolderPath + "AccentBackgroundCold",
                ButtonType.Neutral => Configurations.MaterialsFolderPath + "PrimaryBackground",
                ButtonType.Negative => Configurations.MaterialsFolderPath + "AccentBackground2",
                _ => throw new ApplicationException("Unsupported type")
            }; 
            
            _background.material = Utils.LoadResource<Material>(pathBackground);
            
            string pathStroke = buttonType switch
            {
                ButtonType.Positive => Configurations.MaterialsFolderPath + "AccentBackgroundCold",
                ButtonType.Neutral => Configurations.MaterialsFolderPath + "PrimaryBackground",
                ButtonType.Negative => Configurations.MaterialsFolderPath + "AccentBackground2",
                _ => throw new ApplicationException("Unsupported type")
            }; 
            
            _background.material = Utils.LoadResource<Material>(pathStroke);
        }
        
        public void ChangeStrokeMaterial()
        {
            
        }
    }
}