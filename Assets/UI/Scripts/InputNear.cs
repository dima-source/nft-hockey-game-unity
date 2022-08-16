using System;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    public sealed class InputNear : Input
    {
        [SerializeField]
        private string suffix = "";
        
        [Header("Limits")]
        [SerializeField]
        [Range(1, 10)]
        private int integerLimit;
        [SerializeField]
        [Range(0, 10)]
        private int fractionalLimit;
        
        private const string SEPARATOR = ".";

        public double Value
        {
            get
            {
                int length = Math.Max(0, textView.text.Length - suffix.Length);
                string textValue = textView.text.Substring(length);
                if (Double.TryParse(textValue, out double value))
                {
                    return value;
                }
                
                return 0;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (suffix.Contains(SEPARATOR))
            {
                throw new ApplicationException($"After cannot contain '{SEPARATOR}'");
            }
            
            UpdateLimits();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            UpdateLimits();
        }

        protected override void OnValueChanged(string value)
        {
            int index = textView.text.IndexOf(suffix, StringComparison.Ordinal);

            if (index != -1)
            {
                textView.text = textView.text.Substring(0, index + suffix.Length);
            }
            
            // Adds Near sign to the end
            if (index == -1)
            {
                textView.text = value + suffix;
            }
            else if (index != value.Length - suffix.Length)
            {
                textView.text = textView.text.Remove(index, suffix.Length) + suffix;
            }

            // Updates separator
            int realLength = textView.text.Length - suffix.Length;
            string modifiedText = textView.text.Replace(SEPARATOR, "");
            if (realLength > integerLimit)
            {
                modifiedText = modifiedText.Insert(integerLimit, SEPARATOR);
            }

            textView.text = modifiedText;
        }

        protected override void OnSubmit(string value)
        {
            if (value.Contains(suffix) && value.Length == suffix.Length)
            {
                textView.text = String.Empty;
                return;
            }
            
            int realLength = textView.text.Length - suffix.Length;
            double dValue = Double.Parse(textView.text.Substring(0, realLength));
            string format = "{0:F" + fractionalLimit + "}";
            string toDisplay = String.Format(format, dValue) + suffix;
            textView.text = toDisplay;
        }
        
        private void UpdateLimits()
        {
            inputView.contentType = TMP_InputField.ContentType.IntegerNumber;
            int limit;
            if (fractionalLimit == 0)
            {
                textView.text = "0 " + suffix; 
                limit = integerLimit;
            }
            else
            {
                textView.text = "0.0 " + suffix; 
                limit = integerLimit + fractionalLimit + 1;
            }
            
            inputView.characterLimit = limit + suffix.Length;
        }
    }
}
