using UI.Scripts;
using UnityEngine;

namespace GameScene.UI
{
    public class SwitchOpponentTeam : UiComponent
    {
        private SwitchToggle _toggle;
        private Transform _opponentTeam;
        private Transform _actionMessages;
        
        protected override void Initialize()
        {
            _toggle = global::UI.Scripts.UiUtils.FindChild<SwitchToggle>(transform, "SwitchToggle");
            _opponentTeam = global::UI.Scripts.UiUtils.FindChild<Transform>(transform, "OpponentTeam");
            _actionMessages = global::UI.Scripts.UiUtils.FindChild<Transform>(transform, "ActionMessages");
        }

        protected override void OnUpdate()
        {
            if (_toggle.IsActive)
            {
                _opponentTeam.gameObject.SetActive(true);
                _actionMessages.gameObject.SetActive(false);
            }
            else
            {
                _opponentTeam.gameObject.SetActive(false);
                _actionMessages.gameObject.SetActive(true);
            }
        }
    }
}