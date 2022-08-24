using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class PolygonDrawer : MonoBehaviour
    {

        [SerializeField] private int polygonLineThickness = 30;
        [SerializeField] private int basePolygons = 5;
        [Range(5, 150)]
        [SerializeField] 
        private int vertexRadius = 30;
        
        [Range(0.0f, 1.0f)]
        [SerializeField] 
        private float fill = 0.7f;
        
        [SerializeField] 
        private TMP_FontAsset textFont;

        [SerializeField] private Color concaveColor;
        

        [Serializable]
        public class Statistic
        {
            public string label;
            public int value;

            public Statistic(string label, int value)
            {
                this.label = label;
                if (value < MIN_STAT_VALUE || value > MAX_STAT_VALUE)
                {
                    throw new ApplicationException();
                }
                this.value = value;
            }
        }
        
        public List<Statistic> statistics;


        private const int MIN_STAT_VALUE = 0;
        private const int MAX_STAT_VALUE = 99;
        
        private void OnEnable()
        {
            StartCoroutine(GetComponent<RectTransform>().GetSize(GenerateBase));
        }

        private void OnDisable()
        {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }

        private void GenerateBase(Vector2 initialSize)
        {
            Vector2 deltaSize = initialSize * fill / basePolygons;
            Vector2 size = initialSize;
            
            CircleRenderer maxPolygon = GeneratePolygon(0, size);
            size -= deltaSize;
            for (int i = 1; i < basePolygons - 1; i++)
            {
                GeneratePolygon(i, size);
                size -= deltaSize;
            }
            CircleRenderer minPolygon = GeneratePolygon(0, size);

            List<Vector2> vertexPositions = new List<Vector2>(statistics.Count);
            for (int i = 0; i < statistics.Count; i++)
            {
                Vector2 start = maxPolygon.GetCorner(i);
                Vector2 end = minPolygon.GetCorner(i);
                CircleRenderer circleRenderer = GenerateVertex(i, vertexRadius, start, end);
                vertexPositions.Add(circleRenderer.GetComponent<RectTransform>().anchoredPosition);
            }
            
            GenerateConvexPolygon(initialSize, vertexPositions);
            GenerateOverall(30);
        }

        private CircleRenderer GeneratePolygon(int id, Vector2 size)
        {
            CircleRenderer circleRenderer = GenerateCircle($"Polygon{id}", size, 
                statistics.Count, polygonLineThickness);
            circleRenderer.color = Color.gray;
            return circleRenderer;
        }

        private TextMeshProUGUI GenerateOverall(int textSize)
        {
            int sum = Mathf.RoundToInt(statistics.Select(x => (float) x.value).Sum() / statistics.Count);
            RectTransform rectTransform = GetComponent<RectTransform>();
            TextMeshProUGUI overall = GenerateText("Overall", $"<size={textSize * 2.5f}>{sum}</size><br>overall", 
                rectTransform, textSize);
            return overall;
        }

        private CircleRenderer GenerateVertex(int index, float size, Vector2 start, Vector2 end)
        {
            CircleRenderer circleRenderer = GenerateCircle($"Vertex{index}", 
                new Vector2(size, size), 180, 0);
            circleRenderer.fill = true;
            circleRenderer.color = Color.black;
            
            float scale = (MAX_STAT_VALUE - statistics[index].value) / (float) (MAX_STAT_VALUE - MIN_STAT_VALUE);
            float radius = Vector2.Distance(start, end) * scale;
            float angle = index * Mathf.PI * 2 / statistics.Count;
            Vector2 position = Utils.ToCartesian(radius, angle);
            
            RectTransform rectTransform = circleRenderer.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = start + position;
            GenerateText("Statistic", statistics[index].value.ToString(), rectTransform);
            
            
            Vector2 labelSize = new Vector2(150, 100);
            int labelTextSize = 30;
            Vector2 labelPosition = Utils.ToCartesian(size * 1.5f, angle) * -1;

            TextMeshProUGUI label = GenerateText("Label", statistics[index].label, rectTransform, labelTextSize);
            RectTransform labelRect = label.GetComponent<RectTransform>();
            labelRect.sizeDelta = labelSize;
            labelRect.anchoredPosition = labelPosition;
            label.enableAutoSizing = true;
            label.fontSizeMax = labelTextSize;
            
            return circleRenderer;
        }

        private TextMeshProUGUI GenerateText(string goName, string text, RectTransform parent, int textSize = 0)
        {
            GameObject go = new GameObject(goName);
            go.transform.SetParent(parent, false);

            TextMeshProUGUI textMeshPro = go.AddComponent<TextMeshProUGUI>();
            textMeshPro.fontSizeMin = 5;
            textMeshPro.fontSizeMax = 100;

            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = parent.sizeDelta;
            
            textMeshPro.font = textFont;
            textMeshPro.verticalAlignment = VerticalAlignmentOptions.Middle;
            textMeshPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
            textMeshPro.text = text;

            if (textSize == 0)
            {
                textMeshPro.enableAutoSizing = true;
                const float marginCoefficient = 0.2f; 
                float x = parent.sizeDelta.x * marginCoefficient;
                float y = parent.sizeDelta.y * marginCoefficient;
                textMeshPro.margin = new Vector4(x, y, x, y);
            }
            else
            {
                textMeshPro.fontSize = textSize;
            }

            return textMeshPro;
        }

        private void GenerateConvexPolygon(Vector2 initialSize, List<Vector2> positions)
        {
            GameObject go = new GameObject("Concave");
            go.transform.SetParent(transform, false);
            go.transform.SetSiblingIndex(Math.Max(0, transform.childCount - statistics.Count - 1));

            ConcavePolygonRenderer concave = go.AddComponent<ConcavePolygonRenderer>();
            concave.color = concaveColor;
            concave.positions = positions;
            
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = initialSize;
        }

        private CircleRenderer GenerateCircle(string objName, Vector2 size, 
            int segments, int thickness, Transform parent = null)
        {
            GameObject go = new GameObject(objName);
            go.transform.SetParent(parent ? parent : transform, false);
            
            CircleRenderer baseRenderer = go.AddComponent<CircleRenderer>();
            baseRenderer.segments = segments;
            baseRenderer.thickness = thickness;
            
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;

            return baseRenderer;
        }

    }
}
