using System;
using UnityEngine;

namespace UI.Scripts
{
    [ExecuteInEditMode]
    public abstract class UiComponent : MonoBehaviour
    {
        protected abstract void Initialize();

        protected virtual void OnAwake() { }
        
        protected virtual void OnUpdate() { }

        private void Awake()
        {
            Initialize();
            OnAwake();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Initialize();
        }
#endif
        
        private void Update()
        {
            OnUpdate();
        }
        
    }
}
