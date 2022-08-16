using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Scripts
{
    public class Toggle : UiComponent, IPointerClickHandler
    {

        public string text = "";
        
        [SerializeField]
        private bool _isOn = true;

        public bool isOn => _isOn;
        
        private TextInformation _text;
        private Image _checkmark;
        
        protected override void Initialize()
        {
            _text = Utils.FindChild<TextInformation>(transform, "Label");
            _checkmark = Utils.FindChild<Image>(transform, "Checkmark");
        }

        protected override void OnUpdate()
        {
            _text.text = text;
            _checkmark.gameObject.SetActive(_isOn);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _isOn = !_isOn;
        }
    }
}
