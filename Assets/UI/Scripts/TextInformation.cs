using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    [RequireComponent(typeof(Image))]
    public class TextInformation : UiComponent
    {

        private const float MIN_BORDER_RANGE = 0.01f;
        private const float MAX_BORDER_RANGE = 20.0f;
        
        public enum BackgroundType
        {
            Rectangle,
            RoundedCorners,
            LeafCorners
        }
        
        public enum BackgroundMaterial
        {
            PrimaryBackground,
            SecondaryBackground,
            AccentBackgroundHot,
            AccentBackgroundCold
        }

        [Header("Dependencies")] 
        [SerializeField]
        public TextMeshProUGUI _text;
        
        [Header("Text")]
        public string text;
        public FontStyles fontStyle;
        public bool oneLined;
        
        [Header("Background")] 
        public bool displayed;
        public BackgroundType type = BackgroundType.Rectangle;

        [Range(MIN_BORDER_RANGE, MAX_BORDER_RANGE)]
        public float borderRadius = MAX_BORDER_RANGE;
        
        public BackgroundMaterial material = BackgroundMaterial.PrimaryBackground;
        
        [Range(0.0f, 1.0f)] 
        public float alpha = 1.0f;
        
        
        private Sprite _backgroundSprite;
        private Material _backgroundMaterial;
        
        private Image _background;

        protected override void Initialize()
        {
            _background = gameObject.GetComponent<Image>();
            _backgroundSprite = Utils.LoadSprite(ConvertToPath(type));
            _backgroundMaterial = Utils.LoadResource<Material>(ConvertToPath(material));
            _background.type = Image.Type.Sliced;
        }

        protected override void OnUpdate()
        {
            _text.text = text;
            _text.fontStyle = fontStyle;
            _text.enableWordWrapping = !oneLined;
            _background.enabled = displayed;
            _background.sprite = _backgroundSprite;
            _background.material = _backgroundMaterial;

            Color color = _background.color;
            color.a = alpha;
            _background.color = color;
            
            _background.pixelsPerUnitMultiplier = MAX_BORDER_RANGE / borderRadius;
        }

        private static string ConvertToPath(BackgroundType type)
        {
            return type switch
            {
                BackgroundType.Rectangle => Configurations.SpritesFolderPath + "SpriteSheet/Square",
                BackgroundType.RoundedCorners => Configurations.SpritesFolderPath + "SpriteSheet/Circle",
                BackgroundType.LeafCorners => Configurations.SpritesFolderPath + "SpriteSheet/Leaf",
                _ => throw new ApplicationException("Unsupported type")
            };
        }
        
        private static string ConvertToPath(BackgroundMaterial material)
        {
            return material switch
            {
                BackgroundMaterial.PrimaryBackground => Configurations.MaterialsFolderPath + "PrimaryBackground",
                BackgroundMaterial.SecondaryBackground => Configurations.MaterialsFolderPath + "SecondaryBackground",
                BackgroundMaterial.AccentBackgroundHot => Configurations.MaterialsFolderPath + "AccentBackgroundHot",
                BackgroundMaterial.AccentBackgroundCold => Configurations.MaterialsFolderPath + "AccentBackgroundCold",
                _ => throw new ApplicationException("Unsupported material")
            };
        }
    }
}
