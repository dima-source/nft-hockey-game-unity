using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Scripts
{
    [RequireComponent(typeof(Button))]
    public class UiButton : TextInformation
    {
        private Button _button;

        private const string DefaultSound = "click v1";
        
        protected override void Initialize()
        {
            base.Initialize();
            _button = gameObject.GetComponent<Button>();
            _button.targetGraphic = gameObject.GetComponent<Image>();
        }

        protected sealed override void OnAwake()
        {
            base.OnAwake();
            _button.onClick.AddListener(() =>
            {
                AudioController.LoadClip(Configurations.MusicFolderPath + DefaultSound);
                AudioController.source.Play();
            });
        }

        public void AddOnClickListener(UnityAction action) => _button.onClick.AddListener(action);

    }
}