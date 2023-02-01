using System.Collections.Generic;
using UnityEngine;

namespace UI.Scripts
{
    public class StraightTrapezoidRenderer : RectRenderer
    {
        
        public enum Corner
        {
            BottomLeft,
            BottomRight,
            TopRight,
            TopLeft
        }

        public Corner corner;
        public float intensity;

        protected override void Start()
        {
            intensity = Mathf.Clamp(intensity, 0.0f, MAX_ANGLE);
            
            cornerOffsets = new Vector2[4];   
            MowCorner();
            
            base.Start();
        }
        
        private void MowCorner()
        {
            Vector2 offset = new Vector2(intensity, 0.0f);
            cornerOffsets[(int)corner] = offset;
        }
        
    }
}