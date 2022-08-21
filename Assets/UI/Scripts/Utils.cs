using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Scripts
{
    public static class Utils
    {
        
        private static readonly Dictionary<string, Object> resourcesCache = new();
        
        public static T FindChild<T>(Transform parent, string childName)
        {
            Transform[] children = parent.GetComponentsInChildren<Transform>(true)
                .Where(t => t.name == childName)
                .ToArray();
            
            if (children == null || children.Length == 0)
            {
                throw new ApplicationException($"Child with name '{childName}' does not exist");
            }

            // TODO: think if we should allow to have multiple children with the same name
            if (children.Length > 1)
            {
                throw new ApplicationException($"Multiple children with the same name '{childName}' found");
            }
            
            T component = children[0].GetComponent<T>();

            if (component == null)
            {
                throw new ApplicationException($"Child '{childName}' has not got component '{typeof(T).Name}'");
            }
            
            return component;
        }

        public static T LoadResource<T>(string path) where T : Object
        {
            T resource;
            if (resourcesCache.ContainsKey(path))
            {
                resource = (T)resourcesCache[path];
            }
            else
            {
                resource = Resources.Load<T>(path);
                if (resource == null)
                {
                    throw new ApplicationException($"Resource '{path}' does not exist");
                }
                
                resourcesCache.Add(path, resource);
            }

            return resource;
        }

        // Specific logic to catch the case when sprite is in a SpriteSheet
        public static Sprite LoadSprite(string path)
        {
            Sprite sprite = null;
            try
            {
                // Firstly trying normal loader
                sprite = LoadResource<Sprite>(path);
            }
            catch (ApplicationException e)
            {
                // If sprite is in a SpriteSheet or it doesn't exist
                // From documentation: All asset names and paths in Unity use forward slashes
                int last = path.LastIndexOf('/');
                string spriteSheetName = path.Substring(0, last);
                string spriteName = path.Substring(last + 1);
                Sprite[] sprites = Resources.LoadAll<Sprite>(spriteSheetName);
                
                if (sprites == null || sprites.Length == 0)
                {
                    throw new ApplicationException(e.Message);
                }
                
                // TODO: should we load all sprites from a SpriteSheet first time this function called?
                foreach (Sprite value in sprites)
                {
                    if (!resourcesCache.ContainsKey(spriteSheetName + '/' + value.name))
                    {
                        resourcesCache.Add(spriteSheetName + '/' + value.name, value);   
                    }
                    if (value.name == spriteName)
                    {
                        sprite = value;
                    }
                }
                
                if (sprite == null) {  throw new ApplicationException(e.Message); }
                
            }

            return sprite;
        }
        
        public static Vector2 ToCartesian(float radius, float angle)
        {
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            return new Vector2(x, y);
        }

    }   
}
