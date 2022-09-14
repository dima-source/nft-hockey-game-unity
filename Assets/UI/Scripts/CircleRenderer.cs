using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class CircleRenderer : Graphic
    {
        public bool fill;
        public int thickness = 5;
        [Range(3, 360)]
        public int segments = 360;
        [Range(0, 360)]
        public float rotation;
        [Range(0.0f, 1.0f)]
        public float[] vertexDistances = new float[3];

        private void Update()
        {
            segments = Mathf.Clamp(segments, 3, 360);
            thickness = (int)Mathf.Clamp(thickness, 1, rectTransform.rect.width / 2);
        }

        private UIVertex[] GetQuad(Vector2[] positions)
        {
            Vector2[] uvs = {
                new(0, 0),
                new(0, 1),
                new(1, 1),
                new(1, 0)
            };
            
            UIVertex[] uiVertices = new UIVertex[4];
            for (int i = 0; i < positions.Length; i++)
            {
                UIVertex vertex = UIVertex.simpleVert;
                vertex.color = color;
                vertex.position = positions[i];
                vertex.uv0 = uvs[i];
                uiVertices[i] = vertex;
            }
            return uiVertices;
        }
        
        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear();
            
            Vector2 prevX = Vector2.zero;
            Vector2 prevY = Vector2.zero;

            float degrees = 360f / segments;
            
            
            if (vertexDistances.Length != segments)
            {
                vertexDistances = new float[segments];
                for (int i = 0; i < segments; i++)
                {
                    vertexDistances[i] = 1.0f;
                }
            }
            
            for (int i = 0; i < segments + 1; i++)
            {
                float rad = Mathf.Deg2Rad * (i * degrees + rotation);
                float cos = Mathf.Cos(rad);
                float sin = Mathf.Sin(rad);
                
                float offset = i == segments ? vertexDistances[0] : vertexDistances[i];
                
                float outer = CalculateOuterBoundSize(offset);
                float inner = CalculateInnerBoundSize(offset);
                Vector2[] positions = new Vector2[4];
                positions[0] = prevX;
                positions[1] = new Vector2(outer * cos, outer * sin);
                positions[2] = fill ? Vector2.zero : new Vector2(inner * cos, inner * sin);
                positions[3] = fill ? Vector2.zero : prevY;

                prevX = positions[1];
                prevY = positions[2];
                helper.AddUIVertexQuad(GetQuad(positions));
            }
        }

        private float CalculateOuterBoundSize(float offset = 1.0f)
        {
            offset = Math.Clamp(offset, 0.0f, 1.0f);
            if (rectTransform.rect.width <= rectTransform.rect.height)
            {
                return -rectTransform.pivot.x * offset * rectTransform.rect.width;
            }
                
            return -rectTransform.pivot.y * offset * rectTransform.rect.height;
        }
        
        private float CalculateInnerBoundSize(float offset = 1.0f)
        {
            return CalculateOuterBoundSize(offset) + thickness;
        }

        public Vector2 GetCorner(int segment)
        {
            float angle = 2 * Mathf.PI * (segment % segments) / segments;
            float radius = (CalculateOuterBoundSize() + CalculateInnerBoundSize()) / 2;
            return Utils.ToCartesian(radius, angle);
        }
        
    }
}