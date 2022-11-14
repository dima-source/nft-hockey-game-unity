using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public class MoveTest : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly int _numberOfVectors;

        public MoveTest(Vector3 startCoordinates, Vector3 destinationCoordinates, int numberOfVectors)
        {
            _startCoordinates = startCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            var convertedZ = TrajectoryUtils.GetConvertedZDestination(_startCoordinates, _destinationCoordinates);
            var distance = convertedZ - _startCoordinates.z;

            var numberOfSplinePoints = 5;
            var stepSpline = distance / numberOfSplinePoints;

            var zSpline = new float[numberOfSplinePoints];
            var xSpline = new float[numberOfSplinePoints];

            zSpline[0] = _startCoordinates.z;
            xSpline[0] = _startCoordinates.x;
            
            for (int i = 1; i < numberOfSplinePoints; i++)
            {
                zSpline[i] = zSpline[i-1] + stepSpline;
                xSpline[i] = Random.Range(-1.5f, 1.5f);
            }

            CubicSpline spline = new CubicSpline();
            spline.BuildSpline(zSpline, xSpline, numberOfSplinePoints);

            var result = new List<Vector3>();
            var step = distance / _numberOfVectors;
            var currentCoordinates = _startCoordinates;
            for (int i = 0; i < _numberOfVectors; i++)
            {
                currentCoordinates.z += step;
                currentCoordinates.x = (float)spline.Interpolate(currentCoordinates.z);
                result.Add(currentCoordinates);
            }

            result = TrajectoryUtils.RotateTrajectory(result, _destinationCoordinates);

            return result;
        }
    }
}