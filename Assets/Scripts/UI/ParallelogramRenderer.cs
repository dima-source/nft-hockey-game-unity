using UnityEngine;

namespace UI.Scripts
{
    public class ParallelogramRenderer : RectRenderer
    {
        
        public enum Direction
        {
            HorizontalTopLeftToBottomRight,
            HorizontalTopRightToBottomLeft,
            VerticalTopLeftToBottomRight,
            VerticalTopRightToBottomLeft
        }

        public Direction direction;
        public float intensity;

        protected override void Start()
        {
            intensity = Mathf.Clamp(intensity, 0.0f, MAX_ANGLE);
            
            cornerOffsets = new Vector2[4];   
            MowCorners();
            base.Start();
        }
        

        private void MowCorners()
        {
            if (direction == Direction.HorizontalTopLeftToBottomRight)
            {
                cornerOffsets[0] = new Vector2(0.0f, intensity);
                cornerOffsets[2] = new Vector2(0.0f, intensity);
            }
            else if (direction == Direction.HorizontalTopRightToBottomLeft)
            {
                cornerOffsets[1] = new Vector2(0.0f, intensity);
                cornerOffsets[3] = new Vector2(0.0f, intensity);
            }
            else if (direction == Direction.VerticalTopLeftToBottomRight)
            {
                cornerOffsets[0] = new Vector2(intensity, 0.0f);
                cornerOffsets[2] = new Vector2(intensity, 0.0f);
            }
            else if (direction == Direction.VerticalTopRightToBottomLeft)
            {
                cornerOffsets[1] = new Vector2(intensity, 0.0f);
                cornerOffsets[3] = new Vector2(intensity, 0.0f);
            }
        }
        
    }
}