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
        
        
        private float outer 
        {
            get
            {
                if (rectTransform.rect.width <= rectTransform.rect.height)
                {
                    return -rectTransform.pivot.x * rectTransform.rect.width;
                }
                
                return -rectTransform.pivot.y * rectTransform.rect.height;
            }
        }

        private float inner => outer + thickness;

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
            for (int i = 0; i < segments + 1; i++)
            {
                float rad = Mathf.Deg2Rad * (i * degrees);
                float cos = Mathf.Cos(rad);
                float sin = Mathf.Sin(rad);
            
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


        public Vector2 GetCorner(int segment)
        {
            float angle = 2 * Mathf.PI * (segment % segments) / segments;
            float radius = (outer + inner) / 2;
            return Utils.ToCartesian(radius, angle);
        }
        
    }
}