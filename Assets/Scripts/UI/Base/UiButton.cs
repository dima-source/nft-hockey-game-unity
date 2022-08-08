using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class UiButton : MonoBehaviour
    {
        private Button _button;
        private TextMeshProUGUI _textMeshPro;
        
        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _textMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

#if UNITY_EDITOR
        // Need to guarantee a presence of the TextMeshPro component in children 
        private void OnValidate()
        {
            _button = gameObject.GetComponent<Button>();
            _textMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            if (_textMeshPro == null)
            {
                throw new ApplicationException("Expected a child with the 'TextMeshProUGUI' component");
            }
        }
#endif
    }
}