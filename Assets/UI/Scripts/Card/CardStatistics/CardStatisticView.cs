using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.Card.CardStatistics
{
    
    public class CardStatisticView : UiComponent
    {
        private const float MAX_FILL = 1.0f;
        private static readonly int CRITICAL_CHANGE_DETECTION_PERCENTAGE = 20; 
        
        [Range(0.0f, MAX_FILL)] 
        [SerializeField]
        private float fillAmount = MAX_FILL;
        private float lastFillAmount = MAX_FILL;
        
        public Sprite statisticSprite;
        public bool displayCriticalText;

        private Gradient fillPalette;
        private Image fillBar;
        private Image spriteDisplay;
        private TextMeshProUGUI criticalTextMeshPro;
        
        public float FillAmount
        {
            get => fillAmount;
            set
            {
                lastFillAmount = fillAmount;
                fillAmount = value;
            }
        }

        protected override void Initialize()
        {
            SetEqualDistributedGradientPalette(Color.red, Color.yellow, Color.green);
            fillBar = Utils.FindChild<Image>(transform, "FillStripe");
            fillBar.type = Image.Type.Filled;
            fillBar.fillOrigin = (int)Image.Origin180.Top;
            spriteDisplay = Utils.FindChild<Image>(transform, "SpriteImage");
            criticalTextMeshPro = Utils.FindChild<TextMeshProUGUI>(transform, "BoostText");
        }

        protected override void OnUpdate()
        {
            fillBar.fillAmount = Mathf.Clamp(FillAmount, 0.0f, 1.0f);
            fillBar.color = fillPalette.Evaluate(fillBar.fillAmount);
            spriteDisplay.sprite = statisticSprite;

            if (IsCriticalChange())
            {
                StartCoroutine(ReactOnFillChange(3.0f));
            }
        }
        

        private void SetEqualDistributedGradientPalette(params Color[] colors)
        {
            float step = 1.0f / Math.Max(1, colors.Length - 1);
            GradientColorKey[] keys = new GradientColorKey[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                keys[i] = new GradientColorKey(colors[i], step * i);
            }
            fillPalette = new Gradient {colorKeys = keys};
        }

        private bool IsCriticalChange()
        {
            float change = Math.Abs(FillAmount - lastFillAmount);
            return change / MAX_FILL * 100 >= CRITICAL_CHANGE_DETECTION_PERCENTAGE;
        }

        private IEnumerator ReactOnFillChange(float visibilityDuration)
        {
            GameObject criticalTextObject = criticalTextMeshPro.gameObject;
            if (!displayCriticalText)
            { 
                criticalTextObject.SetActive(false); 
                yield break;
            }

            yield return StartCoroutine(DisplayCriticalText(visibilityDuration));
        }

        private IEnumerator DisplayCriticalText(float visibilityDuration)
        {
            GameObject criticalTextObject = criticalTextMeshPro.gameObject;
            criticalTextObject.SetActive(true);
            if (lastFillAmount < FillAmount)
            {
                criticalTextMeshPro.color = Color.green;
            } 
            else if (lastFillAmount > FillAmount)
            {
                criticalTextMeshPro.color = Color.red;
            }

            yield return new WaitForSeconds(visibilityDuration);
            criticalTextObject.SetActive(false);
        }

    }
    
}