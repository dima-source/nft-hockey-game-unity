using System;
using TMPro;
using UnityEngine;

namespace Base.Prefabs.Inputs
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputNear : MonoBehaviour
    {
        [SerializeField]
        private string after;
        private TMP_InputField _inputField;
        
        public void Awake()
        {
            _inputField = gameObject.GetComponent<TMP_InputField>();
            _inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string value)
        {
            int index = value.IndexOf(after, StringComparison.Ordinal);
            if (index == -1)
            {
                _inputField.text = value + after;   
            }
            else if (index != value.Length - after.Length)
            {
                _inputField.text = _inputField.text.Remove(index, after.Length) + after;   
            }
        }
    }
}
