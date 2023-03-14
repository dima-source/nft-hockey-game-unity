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
            Square,
            Circle,
            Leaf
        }

        [Serializable]
        public class TextView
        {
            [Header("Basic")]
            public string text = "";
            public TMP_FontAsset fontAsset;
            public FontStyles fontStyle;
            public TextAlignmentOptions alignmentOptions;
            
            public float size;
            public bool autoSize;
            public float minSize, maxSize;
            
            [Header("Additional")]
            public bool oneLined;
            [Range(0.0f, 1.0f)]
            public float alpha = 1.0f;

            public void CopyValues(TMP_Text textMeshPro)
            {
                textMeshPro.font = fontAsset;
                textMeshPro.text = text;
                textMeshPro.fontSize = size;
                textMeshPro.fontSizeMin = minSize;
                textMeshPro.fontSizeMax = maxSize;
                textMeshPro.enableAutoSizing = autoSize;
                textMeshPro.spriteAsset = UiUtils.LoadResource<TMP_SpriteAsset>(Configurations.SpritesFolderPath + 
                    "SpriteSheets/UiKitSpriteAsset");
                textMeshPro.alignment = alignmentOptions;
                textMeshPro.fontStyle = fontStyle;
                textMeshPro.enableWordWrapping = !oneLined;
                SetAlpha(textMeshPro, alpha);
            }
        }
        
        [Serializable]
        public class BackgroundView
        {
            public bool displayed;
            public BackgroundType type;
            [Range(MIN_BORDER_RANGE, MAX_BORDER_RANGE)]
            public float borderRadius = MAX_BORDER_RANGE;
            public Material material;
            [Range(0.0f, 1.0f)]
            public float alpha = 1.0f;
            
            public void CopyValues(Image image)
            {
                image.enabled = displayed;
                image.sprite = UiUtils.LoadSprite(ConvertToPath(type));
                image.material = material;
                SetAlpha(image, alpha);
                image.pixelsPerUnitMultiplier = MAX_BORDER_RANGE / borderRadius;
            }
            
            private static string ConvertToPath(BackgroundType type)
            {
                return Configurations.SpritesFolderPath + "SpriteSheets/" + type;
            }
        }
        
        [SerializeField]
        private TextView _textView;
        [SerializeField]
        private BackgroundView _backgroundView;

        public TextView textView => _textView;
        public BackgroundView backgroundView => _backgroundView;
        
        public bool useGlobalAlpha = false;
        [Range(0.0f, 1.0f)]
        public float globalAlpha = 1.0f;

        protected TextMeshProUGUI _textMeshPro;
        private Image _background;

        protected override void Initialize()
        {
            _textMeshPro = UiUtils.FindChild<TextMeshProUGUI>(transform, "Text");
            _background = gameObject.GetComponent<Image>();
            _background.type = Image.Type.Sliced;
        }

        protected override void OnUpdate()
        {
            if (useGlobalAlpha)
            {
                backgroundView.alpha = textView.alpha = globalAlpha;   
            }
            textView.CopyValues(_textMeshPro);
            backgroundView.CopyValues(_background);
        }

        private static void SetAlpha(Graphic graphic, float alpha)
        {
            if (alpha < 0.0f || alpha > 1.0f)
            {
                throw new ApplicationException("Invalid alpha value");
            }
            
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }

        public virtual void SetEnabled(bool value)
        {
            if (!value)
            {
                useGlobalAlpha = true;
                globalAlpha = 0.3f;
            }
            else
            {
                globalAlpha = 1.0f;
            }
        }

    }
}