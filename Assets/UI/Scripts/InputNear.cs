using System;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    public sealed class InputNear : Input
    {
        [SerializeField]
        private string suffix = "";

        public float Value
        {
            get
            {
                int length = Math.Max(0, _inputField.text.Length - suffix.Length);
                string textValue = _inputField.text.Substring(0, length);
                if (float.TryParse(textValue, out float value))
                {
                    return value;
                }
                
                return 0;
            }
        }

        protected override void OnValueChanged(string value)
        {
            int index = _inputField.text.IndexOf(suffix, StringComparison.Ordinal);

            if (index != -1)
            {
                _inputField.SetTextWithoutNotify(_inputField.text.Substring(0, index + suffix.Length));
            }
            
            // Adds Near sign to the end
            if (index == -1)
            {
                _inputField.SetTextWithoutNotify(value + suffix);
            }
            else if (index != value.Length - suffix.Length)
            {
                _inputField.SetTextWithoutNotify(_inputField.text.Remove(index, suffix.Length) + suffix);
            }
        }

    }
}
