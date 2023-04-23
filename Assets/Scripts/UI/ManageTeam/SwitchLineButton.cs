using TMPro;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.ManageTeam
{
    public class SwitchLineButton: UiComponent
    {
        private Button _buttonComponent;
        [SerializeField] private TMP_Text buttonTextObject;
        [SerializeField] public string buttonText;
        [SerializeField] public LineNumbers lineToShow;

        protected override void Initialize()
        {
            _buttonComponent = GetComponent<Button>();
            buttonTextObject = Scripts.Utils.FindChild<TMP_Text>(transform, "ButtonText");
        }

        protected override void OnUpdate()
        {
            buttonTextObject.text = buttonText;
        }
        
        public void ClearCallbacks()
        {
            _buttonComponent.onClick.RemoveAllListeners();
        }

        // TODO: add highlighting button when chosen
        
        public void SetCallback(UnityAction callback)
        {
            _buttonComponent.onClick.AddListener(callback);
        }
    }
}