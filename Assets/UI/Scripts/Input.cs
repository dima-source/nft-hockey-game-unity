using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    [RequireComponent(typeof(TMP_InputField))]
    public class Input : TextInformation
    {
        private TMP_InputField _inputField;

        [Header("Input")] 
        [Range(0, 100)]
        public int characterLimit = 0;
        public TMP_InputField.ContentType contentType;
        [Range(1, 10)]
        public int caretWidth = 1;
        
        public string InputText
        {
            get => _inputField.text;
            set => _inputField.SetTextWithoutNotify(value);
        }

        protected override void Initialize()
        {
            base.Initialize();
            _inputField = gameObject.GetComponent<TMP_InputField>();
            _inputField.placeholder = _text;
            RectTransform textArea = Utils.FindChild<RectTransform>(transform, "TextArea");
            _inputField.textViewport = textArea;
            _inputField.textComponent = Utils.FindChild<TextMeshProUGUI>(textArea, "Text");
            
            _inputField.onValueChanged.AddListener(OnValueChanged);
            _inputField.onSubmit.AddListener(OnSubmit);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _inputField.characterLimit = characterLimit;
            _inputField.caretWidth = caretWidth;
            _inputField.contentType = contentType;
        }

        protected virtual void OnValueChanged(string value) { }
        protected virtual void OnSubmit(string value) { }
    }
}
