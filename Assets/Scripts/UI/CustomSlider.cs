using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CustomSlider : UiComponent
    {
        private Transform _columnsTransform;
        private RectTransform _rectTransform;
        private float _handleRadius;
        private Slider _slider;
        private const string ColumnPath = "Prefabs/Slider/Column";
        
        protected override void Initialize()
        {
            var handleAreaTransform = UiUtils.FindChild<Transform>(transform, "Handle Slide Area").transform;
            var handle = UiUtils.FindChild<RectTransform>(handleAreaTransform, "Handle");
            _handleRadius = handle.sizeDelta.x / 2;
            
            _rectTransform = GetComponent<RectTransform>();
            _slider = GetComponent<Slider>();
        }

        protected override void OnUpdate()
        {
            if (UiUtils.HasChild(transform, "Columns"))
            {
                var columns = UiUtils.FindChild<Transform>(transform, "Columns");
                DestroyImmediate(columns.gameObject);
            }
                
            InitColumns();
        }

        private void InitColumns()
        {
            var emptyObject = new GameObject();
            var columns = Instantiate(emptyObject, transform);
            DestroyImmediate(emptyObject);
            columns.transform.SetAsFirstSibling();
            columns.name = "Columns";
            
            var position = _rectTransform.position;
            var sizeDelta = _rectTransform.sizeDelta;
            
            var startX = position.x - sizeDelta.x / 2 + _handleRadius;
            var endX =  position.x + sizeDelta.x / 2 - _handleRadius;
            
            var numberOfColumns = (int)(_slider.maxValue - _slider.minValue);
            var deltaX = (endX - startX) / numberOfColumns;

            var columnPrefab = UiUtils.LoadResource<GameObject>(ColumnPath);
            var currentX = startX;
            var y = position.y;
            
            for (var i = 0; i <= numberOfColumns; i++)
            {
                var column = Instantiate(columnPrefab, columns.transform);
                var rectTransform = column.GetComponent<RectTransform>();
                rectTransform.position = new Vector3(currentX, y, 0);
                currentX += deltaX;
            }
        }
    }
}