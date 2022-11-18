using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class SwitchToggle : UiComponent
    {
        private Button _onActivateButton;
        private Transform _onActive;
        private Button _offActivateButton;
        private Transform _offActive;

        private bool _isActive;
        public bool IsActive => _isActive;
        
        protected override void Initialize()
        {
            _onActivateButton = Utils.FindChild<Button>(transform, "OnActivateButton");
            _onActive = Utils.FindChild<Transform>(transform, "OnActive");
            _offActivateButton = Utils.FindChild<Button>(transform, "OffActivateButton");
            _offActive = Utils.FindChild<Transform>(transform, "OffActive");

            _onActivateButton.onClick.AddListener(On);
            _offActivateButton.onClick.AddListener(Off);
        }

        protected override void OnUpdate()
        {
            if (_isActive)
            {
                _onActivateButton.gameObject.SetActive(false);
                _onActive.gameObject.SetActive(true);
                
                _offActivateButton.gameObject.SetActive(true);
                _offActive.gameObject.SetActive(false);
            }
            else
            {
                _onActivateButton.gameObject.SetActive(true);
                _onActive.gameObject.SetActive(false);
                
                _offActivateButton.gameObject.SetActive(false);
                _offActive.gameObject.SetActive(true);
            }
        }
        
        private void On()
        {
            _isActive = true;
        }

        private void Off()
        {
            _isActive = false;
        }
    }
}