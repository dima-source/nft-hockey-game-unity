using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class SwitchToggle : UiComponent
    {
        private Button _onActiveButton;
        private Transform _onInactive;
        private Button _offActiveButton;
        private Transform _offInactive;

        private bool _isActive;
        
        protected override void Initialize()
        {
            _onActiveButton = Utils.FindChild<Button>(transform, "OnActiveButton");
            _onInactive = Utils.FindChild<Transform>(transform, "OnInactive");
            _offActiveButton = Utils.FindChild<Button>(transform, "OffActiveButton");
            _offInactive = Utils.FindChild<Transform>(transform, "OffInactive");
            
            _onActiveButton.onClick.AddListener(Off);
            _offActiveButton.onClick.AddListener(On);
        }

        protected override void OnUpdate()
        {
            if (_isActive)
            {
                _onActiveButton.gameObject.SetActive(true);
                _onInactive.gameObject.SetActive(false);
                
                _offActiveButton.gameObject.SetActive(false);
                _offInactive.gameObject.SetActive(true);
            }
            else
            {
                _onActiveButton.gameObject.SetActive(false);
                _onInactive.gameObject.SetActive(true);
                
                _offActiveButton.gameObject.SetActive(true);
                _offInactive.gameObject.SetActive(false);
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