using TMPro;
using UI.Scripts;
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
        private Image _background;

        protected override void Initialize()
        {
            _button = GetComponent<Button>();
            _text =  UiUtils.FindChild<TextMeshProUGUI>(transform, "Text");
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