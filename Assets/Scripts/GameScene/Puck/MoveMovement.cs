using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public class MoveMovement : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly int _numberOfVectors;

        public MoveMovement(Vector3 startCoordinates, Vector3 destinationCoordinates, int numberOfVectors = 1000)
        {
            _startCoordinates = startCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            var convertedZ = Mathf.Abs(TrajectoryUtils.GetConvertedZDestination(_startCoordinates, _destinationCoordinates));

            var numberOfSplinePoints = 5;
            var stepSpline = convertedZ / numberOfSplinePoints;

            var zSpline = new float[numberOfSplinePoints];
            var xSpline = new float[numberOfSplinePoints];

            zSpline[0] = _startCoordinates.z;
            xSpline[0] = _startCoordinates.x;
            
            for (int i = 1; i < numberOfSplinePoints; i++)
            {
                zSpline[i] = zSpline[i-1] + stepSpline;
                xSpline[i] = Random.Range(-2f, 2f);
            }

            CubicSpline spline = new CubicSpline();
            spline.BuildSpline(zSpline, xSpline, numberOfSplinePoints);
            
            var result = new List<Vector3>();
            var step = convertedZ / (_numberOfVectors - 100);
            var currentCoordinates = _startCoordinates;

            bool isMirror = TrajectoryUtils.IsProbabilityInRandomRange(50);
            
            for (int i = 0; i < (_numberOfVectors - 100); i++)
            {
                currentCoordinates.z += step;
                currentCoordinates.x = (float) spline.Interpolate(currentCoordinates.z);
                
                if (isMirror)
                {
                    currentCoordinates.x = TrajectoryUtils.MirrorCoordinate(_startCoordinates.x, currentCoordinates.x);
                }
                
                result.Add(currentCoordinates);
            }

            result = TrajectoryUtils.RotateTrajectory(result, _destinationCoordinates);

            return result;
        }
    }
}