using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Puck
{
    public class PuckTrack : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private List<Vector3> _linePositions;
        private bool _isDrawing;
        
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _linePositions = new List<Vector3>();
            _lineRenderer.startWidth = 0.3f;
            _lineRenderer.endWidth = 0.3f;
            _lineRenderer.positionCount = 0;
        }

        private void Update()
        {
            if (!_isDrawing && _linePositions.Count > 0 || _linePositions.Count > 1000)
            {
                _linePositions.Remove(_linePositions.First());
                _lineRenderer.SetPositions(_linePositions.ToArray());
                _lineRenderer.positionCount--;
            }
        }

        public void DrawTrack(Vector3 trajectory)
        {
            _isDrawing = true;
            _lineRenderer.positionCount += 1;
            _linePositions.Add(trajectory);
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, trajectory);
        }

        public void ClearTrack()
        {
            _lineRenderer.startWidth = 0.1f;
            _isDrawing = false;
        }
    }
}