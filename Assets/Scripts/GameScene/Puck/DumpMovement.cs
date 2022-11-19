using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public class DumpMovement : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly int _numberOfVectors;

        public DumpMovement(Vector3 startCoordinates, Vector3 destinationCoordinates, int numberOfVectors)
        {
            _startCoordinates = startCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            // TODO: The path does not bend
            var zSpline = new [] {-4.96f, -21.70f, -23.56f, -24.43f, -26.3f, -26.99f, -28.36f, -29.2f, -29.85f, -29.92f, _destinationCoordinates.z};
            var xSpline = new[] {14.29f, 14.5f, 14.225f, 13.96f, 12.99f, 12.63f, 11.05f, 9.37f, 6.45f, 5.12f, _destinationCoordinates.x};

            CubicSpline spline = new CubicSpline();
            spline.BuildSpline(zSpline, xSpline, 11);

            var distance = _destinationCoordinates.z - _startCoordinates.z;
            var step = distance / _numberOfVectors;

            var currentCoordinates = _startCoordinates;
            var result = new List<Vector3>();
            for (int i = 0; i < _numberOfVectors; i++)
            {
                currentCoordinates.z += step;
                currentCoordinates.x = (float)spline.Interpolate(currentCoordinates.z);
                
                result.Add(currentCoordinates);
            }

            return result;
        }
    }
}