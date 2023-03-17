using System;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    [RequireComponent(typeof(TMP_InputField))]
    public class Input : TextInformation
    {
        protected TMP_InputField _inputField;
        
        [SerializeField]
        private Color textColor;

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

        private TextMeshProUGUI _input;

        protected override void Initialize()
        {
            base.Initialize();
            _inputField = gameObject.GetComponent<TMP_InputField>();
            _inputField.placeholder = _textMeshPro;
            RectTransform textArea = UiUtils.FindChild<RectTransform>(transform, "TextArea");
            _inputField.textViewport = textArea;
            _input = UiUtils.FindChild<TextMeshProUGUI>(textArea, "Input");
            _inputField.textComponent = _input;
            _inputField.onValueChanged.AddListener(OnValueChanged);
            _inputField.onSubmit.AddListener(OnSubmit);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            textView.CopyValues(_inputField.textComponent);
            _inputField.textComponent.color = textColor;
            _inputField.characterLimit = _inputView.characterLimit;
            _inputField.caretWidth = _inputView.caretWidth;
            _inputField.contentType = _inputView.contentType;
        }

        protected virtual void OnValueChanged(string value) { }
        protected virtual void OnSubmit(string value) { }
    }
}
