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
            var zLenght = TrajectoryUtils.GetZLenght(_destinationPoints, _startPoint);
            var vAvg = zLenght / _numberOfVectors;
            var maxV = Random.Range(10.0f, 20.0f) * vAvg;
            var endV = (1 / Random.Range(3.0f, 4.0f)) * vAvg;

            var accelerationZone = _numberOfVectors / 20.0f;
            var accelerationZoneLenght = zLenght / 20;
            
            var decelerationZone = _numberOfVectors - accelerationZone;
            var decelerationZoneLenght = zLenght - accelerationZoneLenght;
            
            var acceleration = -TrajectoryUtils.GetAcceleration(accelerationZoneLenght, maxV, accelerationZone);
            var deceleration = TrajectoryUtils.GetAcceleration(decelerationZoneLenght, endV, decelerationZone);

            var angle = TrajectoryUtils.GetAngle(_destinationPoints[0], _startPoint);
            var relativeAngle = TrajectoryUtils.GetRelativeAngle(_destinationPoints[0], _startPoint);
            angle += relativeAngle;
            
            var currentVz = 0.0f;
            var currentCoordinates = _startPoint;
            var result = new List<Vector3>();
            for (int i = 0; i < _numberOfVectors; i++)
            {
                if (accelerationZone > i)
                {
                    currentVz += acceleration;
                }
                else
                {
                   currentVz -= deceleration;
                }
                var distance1 = TrajectoryUtils.GetDistance(_destinationPoints[0], currentCoordinates);
                
                currentCoordinates.z += currentVz;
                currentCoordinates.x = Mathf.Tan(angle) * currentCoordinates.z;
                
                var distance2 = TrajectoryUtils.GetDistance(_destinationPoints[0], currentCoordinates);
                if (distance2 > distance1)
                {
                    currentCoordinates.z = _destinationPoints[0].z;
                    currentCoordinates.x = _destinationPoints[0].x;
                    result.Add(currentCoordinates);
                    break;
                }
                result.Add(currentCoordinates);
            }
            
            return result;
        }
    }
}