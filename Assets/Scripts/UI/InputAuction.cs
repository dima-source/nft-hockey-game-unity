using System;
using UnityEngine;
using Input = UI.Scripts.Input;

namespace UI
{
    public class InputAuction : Input
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
    }
}