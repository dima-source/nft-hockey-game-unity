using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class SwitchToggleV2 : UiComponent
    {
        private Button _onButton;
        private Button _offButton;

        private Animation _animation;
        
        private bool _isActive;
        public bool IsActive => _isActive;
        
        private bool _isPlayed = true;
        
        protected override void Initialize()
        {
            _onButton = Utils.FindChild<Button>(transform, "OnButton");
            _offButton = Utils.FindChild<Button>(transform, "OffButton");
            
            _animation = GetComponent<Animation>();
            _onButton.onClick.AddListener(On);
            _offButton.onClick.AddListener(Off);
        }

        protected override void OnUpdate()
        {
            if (_isPlayed) return;

            if (_isActive)
            {
                _animation.Play("OnToggleV2");
                _isPlayed = true;
            }
            else
            {
                _animation.Play("OffToggleV2");
                _isPlayed = true;
            }
        }

        private void On()
        {
            _isActive = true;
            _isPlayed = false;
        }
        
        private void Off()
        {
            _isActive = false;
            _isPlayed = false;
        }
    }
}