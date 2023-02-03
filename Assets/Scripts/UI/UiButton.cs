using TMPro;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class UiButton : UiComponent
    {
        public enum ButtonType
        {
            Positive,
            Neutral,
            Negative
        }
        private const string DefaultSound = "click v1";
        
        private TextMeshProUGUI _text;
        private Button _button;
        private Image _stroke;
        private Image _background;

        private RectTransform _strokeRectTransform;
        private RectTransform _backgroundRectTransform;

        protected override void Initialize()
        {
            _button = GetComponent<Button>();
            _text =  UiUtils.FindChild<TextMeshProUGUI>(transform, "Text");
            
            _stroke = GetComponent<Image>();
            _background = UiUtils.FindChild<Image>(transform, "MainArea");
            
            _strokeRectTransform = GetComponent<RectTransform>();
            _backgroundRectTransform = UiUtils.FindChild<RectTransform>(transform, "MainArea");
        }

        protected override void OnUpdate()
        {
            SetStroke();
        }

        private void SetStroke()
        {
            var sizeDeltaStroke = _strokeRectTransform.sizeDelta;
            float widthStroke = sizeDeltaStroke.x / 30;
            
            _backgroundRectTransform.sizeDelta = new Vector2(sizeDeltaStroke.x - widthStroke,
               sizeDeltaStroke.y - widthStroke);

            _backgroundRectTransform.anchoredPosition = new Vector2(0, 0);
        }
        
        private void SetBackgroundMaterial(string path)
        {
            _background.material = UiUtils.LoadResource<Material>(path);
        }

        private void SetStrokeMaterial(string path)
        {
            _stroke.material = UiUtils.LoadResource<Material>(path);
        }

        private void SetText(string text)
        {
            _text.text = text;
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