using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Scripts
{
    public class RectRenderer : ConcavePolygonRenderer
    {
        
        protected const float MAX_ANGLE = 45.0f;
        
        [VectorRangeAttribute(0.0f, MAX_ANGLE, 0.0f, MAX_ANGLE)]
        public Vector2[] cornerOffsets;


        protected override void Start()
        {
            if (cornerOffsets.Length != 4)
            {
                cornerOffsets = new Vector2[4];   
            }

            SetPositions(CalculatePositions());
            Debug.Log("in start");
        }
        
        protected override void OnValidate()
        {
            Start();
        }

        private List<Vector2> CalculatePositions()
        {
            List<Vector2> positions = new List<Vector2>
            {
                new Vector2(rectTransform.rect.xMin, rectTransform.rect.yMin) * CalculateMowCoefficient(cornerOffsets[0]), 
                new Vector2(rectTransform.rect.xMax, rectTransform.rect.yMin) * CalculateMowCoefficient(cornerOffsets[1]),
                new Vector2(rectTransform.rect.xMax, rectTransform.rect.yMax) * CalculateMowCoefficient(cornerOffsets[2]), 
                new Vector2(rectTransform.rect.xMin, rectTransform.rect.yMax) * CalculateMowCoefficient(cornerOffsets[3]),
            };
            return positions;
        }
        
        private Vector2 CalculateMowCoefficient(Vector2 cornerDecline)
        {
            Vector2 max = new Vector2(MAX_ANGLE, MAX_ANGLE);
            return (max - cornerDecline) / max;
        }
        
    }
}