using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class UiButton : TextInformation, IPointerClickHandler
    {
        public Action onClick { get; set; }
         
        private const string DefaultSound = "click v1";

        protected override void Initialize()
        {
            base.Initialize();
            onClick = () => { };
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.LoadClip(Configurations.MusicFolderPath + DefaultSound);
            AudioController.source.Play();
            onClick?.Invoke();
        }
        
    }
}