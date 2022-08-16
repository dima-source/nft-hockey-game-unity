using System;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    [RequireComponent(typeof(TMP_InputField))]
    public class Input : TextInformation
    {
        private TMP_InputField _inputField;

        [Serializable]
        public sealed class InputView
        {
            [Range(0, 100)]
            public int characterLimit;
            public TMP_InputField.ContentType contentType;
            public int caretWidth = 3;
        }

        [SerializeField]
        private InputView _inputView;
        
        public InputView inputView => _inputView;
        
        protected override void Initialize()
        {
            base.Initialize();
            _inputField = gameObject.GetComponent<TMP_InputField>();
            _inputField.placeholder = _textMeshPro;
            RectTransform textArea = Utils.FindChild<RectTransform>(transform, "TextArea");
            _inputField.textViewport = textArea;
            _inputField.textComponent = Utils.FindChild<TextMeshProUGUI>(textArea, "Input");
            
            _inputField.onValueChanged.AddListener(OnValueChanged);
            _inputField.onSubmit.AddListener(OnSubmit);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            textView.CopyValues(_inputField.textComponent);
            _inputField.textComponent.color = Color.white;
            _inputField.characterLimit = inputView.characterLimit;
            _inputField.caretWidth = inputView.caretWidth;
            _inputField.contentType = inputView.contentType;
        }

        protected virtual void OnValueChanged(string value) { }
        protected virtual void OnSubmit(string value) { }
    }
}
