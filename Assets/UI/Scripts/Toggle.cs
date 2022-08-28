using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Scripts
{
    [RequireComponent(typeof(Image))]
    public class Toggle : UiComponent, IPointerClickHandler
    {

        public string text = "";

        [Range(0, 10)]
        [SerializeField]
        private int spacing;
        
        public bool isOn = true;
        
        [CanBeNull] 
        public Action onChange { get; set; }

        private TextMeshProUGUI _text;

        protected override void Initialize()
        {
            _text = Utils.FindChild<TextMeshProUGUI>(transform, "Label");
        }

        protected override void OnUpdate()
        {
            string space = new String(' ', spacing);
            if (isOn)
            {
                _text.text = "<sprite name=CheckmarkFilled>" + space + text;        
            }
            else
            {
                _text.text = "<sprite name=CheckmarkEmpty>" + space + text;  
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.LoadClip(Configurations.DefaultButtonSoundPath);
            AudioController.source.Play();
            onChange?.Invoke();
            isOn = !isOn;
        }
    }
}
