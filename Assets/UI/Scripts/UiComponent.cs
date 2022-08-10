using System;
using UnityEngine;

namespace UI.Scripts
{
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
            OnUpdate();
        }
#else
        // TODO: Check if it works in the build
        private void Update()
        {
            if (Application.isPlaying)
            {
                OnUpdate();
            }
        }
#endif
        
    }
}
