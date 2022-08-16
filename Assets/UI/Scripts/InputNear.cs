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
                int length = Math.Max(0, InputText.Length - suffix.Length);
                string textValue = InputText.Substring(length);
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
            int index = InputText.IndexOf(suffix, StringComparison.Ordinal);

            if (index != -1)
            {
                InputText = InputText.Substring(0, index + suffix.Length);
            }
            
            // Adds Near sign to the end
            if (index == -1)
            {
               InputText = value + suffix;
            }
            else if (index != value.Length - suffix.Length)
            {
                InputText = InputText.Remove(index, suffix.Length) + suffix;
            }

            // Updates separator
            int realLength = InputText.Length - suffix.Length;
            string modifiedText = InputText.Replace(SEPARATOR, "");
            if (realLength > integerLimit)
            {
                modifiedText = modifiedText.Insert(integerLimit, SEPARATOR);
            }

            InputText = modifiedText;
        }

        protected override void OnSubmit(string value)
        {
            if (value.Contains(suffix) && value.Length == suffix.Length)
            {
                InputText = String.Empty;
                return;
            }
            
            int realLength = InputText.Length - suffix.Length;
            double dValue = Double.Parse(InputText.Substring(0, realLength));
            string format = "{0:F" + fractionalLimit + "}";
            string toDisplay = String.Format(format, dValue) + suffix;
            InputText = toDisplay;
        }
        
        private void UpdateLimits()
        {
            contentType = TMP_InputField.ContentType.IntegerNumber;
            int limit;
            if (fractionalLimit == 0)
            {
                text = "0 " + suffix; 
                limit = integerLimit;
            }
            else
            {
                text = "0.0 " + suffix; 
                limit = integerLimit + fractionalLimit + 1;
            }
            
            characterLimit = limit + suffix.Length;
        }
    }
}
