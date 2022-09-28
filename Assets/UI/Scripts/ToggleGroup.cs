using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Scripts
{
    public class ToggleGroup : UiComponent, IPointerClickHandler
    {

        public string groupName = "Toggle group";
        
        [SerializeField]
        private bool _isOpen = true;
        public bool isOpen => _isOpen;
        
        public Toggle[] _toggles;

        [SerializeField]
        private bool isOneAnswerOnly;

        [SerializeField] 
        private string[] toggles; 

        private TextMeshProUGUI _toggleGroupName;
        private Transform _toggleContainer;
        private TextMeshProUGUI _arrow;

        public Action onChange;

        private static readonly Dictionary<bool, string> ARROWS = new()
        {
            { false, "DownArrow" },
            { true, "TopArrow" }
        };

        protected override void Initialize()
        {
            _toggleGroupName = Utils.FindChild<TextMeshProUGUI>(transform, "ToggleGroupName");
            _toggleContainer = Utils.FindChild<Transform>(transform, "ToggleContainer");
            _toggles = new Toggle[_toggleContainer.childCount];
            for (int i = 0; i < _toggles.Length; i++)
            {
                _toggles[i] = _toggleContainer.GetChild(i).GetComponent<Toggle>();
                _toggles[i].isOn = false;
                if (isOneAnswerOnly)
                {
                    _toggles[i].onChange = () =>
                    {
                        foreach (var toggle in _toggles) { toggle.isOn = false; }
                        onChange?.Invoke();
                    };
                }
                else
                {
                    _toggles[i].onChange = null;
                }
            }
            _arrow = Utils.FindChild<TextMeshProUGUI>(transform, "Arrow");
        }

        public bool[] GetValues()
        {
            return _toggles.Select(toggle => toggle.isOn).ToArray();
        }

        protected override void OnUpdate()
        {
            _toggleGroupName.text = groupName;
            _toggleContainer.gameObject.SetActive(_isOpen);
            _arrow.text = $"<sprite name={ARROWS[_isOpen]}>";
            UpdateToggles();
        }
        
        private void UpdateToggles()
        {
            if (toggles.Length > _toggles.Length)
            {
                toggles = toggles.Take(_toggles.Length).ToArray();
            }
            
            for (int i = 0; i < _toggles.Length; i++)
            {
                GameObject toggleObject = _toggles[i].gameObject;
                toggleObject.SetActive(i < toggles.Length);
                if (i < toggles.Length)
                {
                    _toggles[i].text = toggles[i];
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.LoadClip(Configurations.DefaultButtonSoundPath);
            AudioController.source.Play();
            _isOpen = !_isOpen;
        }
    }
}
