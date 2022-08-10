using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Scripts
{
    public static class AudioController
    {
        public static AudioSource source => CreateSource();

        public static void LoadClip(string path)
        {
            source.clip = Utils.LoadResource<AudioClip>(path);
        }

        private static AudioSource CreateSource()
        {
            GameObject controllerObject = GameObject.Find("AudioController");
            if (controllerObject == null)
            {
                controllerObject = new GameObject("AudioController");
                return controllerObject.AddComponent<AudioSource>();
            }

            AudioSource sourceComponent = controllerObject.GetComponent<AudioSource>();
            if (sourceComponent == null)
            {
                return controllerObject.AddComponent<AudioSource>();
            }
            return sourceComponent;
        }
    }
}
