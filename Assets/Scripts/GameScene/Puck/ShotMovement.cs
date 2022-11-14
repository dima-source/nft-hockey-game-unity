using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public class ShotMovement : IPuckMovement
    {
        private readonly List<Vector3> _destinationPoints;
        private readonly Vector3 _startPoint;
        private readonly Vector3 _interceptionPoint;
        private readonly int _numberOfVectors;

        public ShotMovement(List<Vector3> destinationPoints, Vector3 startPoint, Vector3 interceptionPoint, int numberOfVectors)
        {
            _destinationPoints = destinationPoints;
            _startPoint = startPoint;
            _interceptionPoint = interceptionPoint;
            _numberOfVectors = numberOfVectors;
        }

        public List<Vector3> GetTrajectory()
        {
            var accelerationZone = _numberOfVectors / 100.0f;
            
            
            var currentVz = 0.0f;
            var currentCoordinates = _startPoint;
            var result = new List<Vector3> {_startPoint};
            var convertedZ = TrajectoryUtils.GetConvertedZDestination(_startPoint, _destinationPoints[0]);
            
            // 0.116633698
            var acceleration = 0.0116633698f;
            var deceleration = acceleration / 50;

            if (convertedZ < _startPoint.z)
            {
                acceleration *= -1;
                deceleration *= -1;
            }
            
            for (int i = 1; i < _numberOfVectors; i++)
            {
                if (accelerationZone > i)
                {
                    currentVz += acceleration;
                }
                else
                {
                   currentVz -= deceleration;
                }
                
                currentCoordinates.z += currentVz;
                
                if (Mathf.Abs(convertedZ) <= Mathf.Abs(currentCoordinates.z))
                {
                    break;
                }
                result.Add(currentCoordinates);
            }

            result = TrajectoryUtils.RotateTrajectory(result, _destinationPoints[0]);
            
            
            return result;
        }
    }
}