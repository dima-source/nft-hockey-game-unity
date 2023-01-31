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
        [SerializeField] private TextMeshProUGUI text;
        private Image _stroke;
        private Image _background;
        private float _widthStroke = 5;
        
        public Action ONClick { get; set; }
         
        private const string DefaultSound = "click v1";

        protected override void Initialize()
        {
            _stroke = GetComponent<Image>();
            _background = Utils.FindChild<Image>(transform, "MainArea");
            text =  Utils.FindChild<TextMeshProUGUI>(transform, "Text");
            var strokeRect = GetComponent<RectTransform>();
            var backgroundRect = Utils.FindChild<RectTransform>(transform, "MainArea");
            var sizeDeltaStroke = strokeRect.sizeDelta;
            var sizeDeltaBackground = backgroundRect.sizeDelta;
            _widthStroke = sizeDeltaStroke.x / 40;
            
            backgroundRect.sizeDelta = new Vector2(sizeDeltaStroke.x - _widthStroke,
               sizeDeltaStroke.y - _widthStroke);
       
            
            ONClick = () => { };
            
            SetButtonType(buttonType, text.text);
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
            _background.material = Utils.LoadResource<Material>(pathBackground);

            string pathStroke = ButtonUtils.StrokeMatPath[buttonType];
            _background.material = Utils.LoadResource<Material>(pathStroke);
        }

        private void SetSprites()
        {
            string pathBackground = ButtonUtils.BackgroundSpritePath[buttonType];
            _background.sprite = Utils.LoadResource<Sprite>(pathBackground);

            string pathStroke = ButtonUtils.StrokeSpritePath[buttonType];
            _background.sprite= Utils.LoadResource<Sprite>(pathStroke);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.LoadClip(Configurations.MusicFolderPath + DefaultSound);
            AudioController.source.Play();
            ONClick?.Invoke();
        }
    }
}