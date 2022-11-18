using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class SwitchToggle : UiComponent
    {
        private Button _onActivateButton;
        private Button _offActivateButton;

        private bool _isActive;
        private bool _isAnimationPlayed = true;
        private Animation _animation;
        
        public bool IsActive => _isActive;
        
        
        protected override void Initialize()
        {
            _onActivateButton = Utils.FindChild<Button>(transform, "OnActivateButton");
            _offActivateButton = Utils.FindChild<Button>(transform, "OffActivateButton");

            _animation = GetComponent<Animation>();
            _onActivateButton.onClick.AddListener(On);
            _offActivateButton.onClick.AddListener(Off);
        }

        protected override void OnUpdate()
        {
            if (_isAnimationPlayed) return;

            if (_isActive)
            {
                _animation.Play("OnToggle");
                _isAnimationPlayed = true;
            }
            else
            {
                _animation.Play("OffToggle");
                _isAnimationPlayed = true;
            }
        }
        
        private void On()
        {
            _isActive = true;
            _isAnimationPlayed = false;
        }

        private void Off()
        {
            _isActive = false;
            _isAnimationPlayed = false;
        }
    }
}