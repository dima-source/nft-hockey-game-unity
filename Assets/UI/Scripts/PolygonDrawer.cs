using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Scripts
{
    public class PolygonDrawer : UiComponent
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
            public SubStatistic[] SubStatistics;
            
            public struct SubStatistic
            {
                public string Label;
                public int Value;
            }
            
            public Statistic(string label, int value, SubStatistic[] subStatistics)
            {
                this.label = label;
                if (value is < MIN_STAT_VALUE or > MAX_STAT_VALUE)
                {
                    throw new ApplicationException();
                }
                this.value = value;

                SubStatistics = subStatistics;
            }
        }

        private static readonly string Path = Configurations.PrefabsFolderPath + "InfoPopup";
        private PopupInfo _popupInfo;
        private Button[] _vertexButtons;
        private Transform _vertexButtonsContainer;
        public List<Statistic> statistics;
        

        private const int MIN_STAT_VALUE = 0;
        private const int MAX_STAT_VALUE = 99;

        protected override void Initialize()
        {
            _vertexButtonsContainer = Utils.FindChild<Transform>(transform, "VertexButtonContainer");
            _vertexButtons = new Button[_vertexButtonsContainer.childCount];
            for (int i = 0; i < _vertexButtonsContainer.childCount; i++)
            {
                _vertexButtons[i] = _vertexButtonsContainer.GetChild(i).GetComponent<Button>();
            }
        }
        
        protected override void OnUpdate()
        {
            _vertexButtonsContainer.transform.SetAsLastSibling();
        }
        
        private void OnEnable()
        {
            StartCoroutine(GetComponent<RectTransform>().GetSize(GenerateBase));
        }

        private void OnDisable()
        {
            foreach (Transform child in transform) {
                if (child.name != "VertexButtonContainer" && child.name != "InfoPopup")
                {
                    Debug.Log("Destroy");
                    Destroy(child.gameObject);
                }
            }

            foreach (var button in _vertexButtons)
            {
                button.gameObject.SetActive(false);
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

                var index = i;
                CircleRenderer circleRenderer = GenerateVertex(i, vertexRadius, start, end, () =>
                {
                    ShowStatsPopup(index);
                });
                
                vertexPositions.Add(circleRenderer.GetComponent<RectTransform>().anchoredPosition);
            }
            
            GenerateConvexPolygon(initialSize, vertexPositions);
            GenerateOverall(30);
        }

        private void ShowStatsPopup(int index)
        {
            if (_popupInfo != null)
            {
                Destroy(_popupInfo.gameObject);
            }
            
            GameObject prefab = Utils.LoadResource<GameObject>(Path);
            _popupInfo = Instantiate(prefab, transform).GetComponent<PopupInfo>();
            var popupRectTransform = _popupInfo.GetComponent<RectTransform>();
            
            CircleRenderer vertex = Utils.FindChild<CircleRenderer>(transform, $"Vertex{index}");
            var vertexRectTransform = vertex.GetComponent<RectTransform>();

            var anchoredPosition = vertexRectTransform.anchoredPosition;
            var sizeDelta = popupRectTransform.sizeDelta;
            popupRectTransform.anchoredPosition = new Vector2(
                (float) (anchoredPosition.x - sizeDelta.x / 1.5),
                (float) (anchoredPosition.y )
                );

            var info = "";
            foreach (var statistic in statistics[index].SubStatistics)
            {
                info += statistic.Label + ": " + statistic.Value + "\n";
            }
            _popupInfo.SetTitle(statistics[index].label + ": "+ statistics[index].value);
            
            _popupInfo.SetInfo(info);
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

        private CircleRenderer GenerateVertex(int index, float size, Vector2 start, Vector2 end, UnityAction action = null)
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
            
            if (action != null)
            {
                _vertexButtons[index].gameObject.SetActive(true);
                
                var buttonRectTransform = _vertexButtons[index].GetComponent<RectTransform>();
                buttonRectTransform.anchoredPosition = rectTransform.anchoredPosition;
                
                _vertexButtons[index].onClick.AddListener(() =>
                {
                    action.Invoke();
                });
            }
            
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
            concave.SetPositions(positions);
            
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
