using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Puck
{
    public class ShotMovement : IPuckMovement
    {
        private readonly List<Vector3> _destinationPoints;
        private readonly Vector3 _startPoint;
        private readonly int _numberOfVectors;
        private float _acceleration;

        public ShotMovement(List<Vector3> destinationPoints, Vector3 startPoint, int numberOfVectors, float acceleration = 0.0116633698f)
        {
            _destinationPoints = destinationPoints;
            _startPoint = startPoint;
            _numberOfVectors = numberOfVectors;
            _acceleration = acceleration;
        }

        public List<Vector3> GetTrajectory()
        {
            if (_destinationPoints == null || _destinationPoints.Count == 0) return new List<Vector3>();
            
            var accelerationZone = _numberOfVectors / 100.0f;
            
            var currentVz = 0.0f;
            var currentCoordinates = _startPoint;
            var startCoordinates = _startPoint;
            var result = new List<Vector3> {_startPoint};
            
            var deceleration = _acceleration / 120;

            var convertedZ = TrajectoryUtils.GetConvertedZDestination(_startPoint, _destinationPoints[0]);
            if (convertedZ < _startPoint.z)
            {
                _acceleration *= -1;
                deceleration *= -1;
            }

            var currentTrajectory = new List<Vector3>();
            var currentDestinationIndex = 0;
            for (int i = 1; i < _numberOfVectors; i++)
            {
                if (accelerationZone > i)
                { 
                    currentVz += _acceleration;
                }
                else
                {
                    if (currentVz >= 0 && currentVz - deceleration <= 0)
                    {
                        currentVz = 0;
                    } else if (currentVz <= 0 && currentVz - deceleration >= 0)
                    {
                        currentVz = 0;
                    }

                    if (currentVz != 0)
                    {
                        currentVz -= deceleration;
                    }
                }
            
                currentCoordinates.z += currentVz;

                if (CheckDestination(startCoordinates.z, convertedZ, currentCoordinates.z))
                {
                    currentCoordinates.z = convertedZ;
                    currentTrajectory.Add(currentCoordinates);
                    
                    currentTrajectory = TrajectoryUtils
                        .RotateTrajectory(currentTrajectory, _destinationPoints[currentDestinationIndex]);
                    result.AddRange(currentTrajectory);
                 
                    currentDestinationIndex++;
                    if (currentDestinationIndex >= _destinationPoints.Count)
                    {
                        currentTrajectory.Clear();
                        break;
                    }

                    startCoordinates = currentTrajectory.Last();
                    currentCoordinates = startCoordinates;
                    convertedZ = TrajectoryUtils
                        .GetConvertedZDestination(startCoordinates, _destinationPoints[currentDestinationIndex]);
                    
                    if (convertedZ < startCoordinates.z && _acceleration > 0)
                    {
                        _acceleration *= -1;
                        deceleration *= -1;
                        currentVz *= -1;
                    } else if (convertedZ > startCoordinates.z && _acceleration < 0)
                    {
                        _acceleration *= -1;
                        deceleration *= -1;
                        currentVz *= -1;
                    }
                
                    currentTrajectory.Clear();
                    
                    continue;
                }
                
                currentTrajectory.Add(currentCoordinates);
            }
            
            currentTrajectory = TrajectoryUtils
                .RotateTrajectory(currentTrajectory, _destinationPoints.Last());
            result.AddRange(currentTrajectory);
            
            return result;
        }

        private bool CheckDestination(float zStart, float zDestination, float zCurrent)
        {
            if (zStart <= zDestination && zCurrent >= zDestination)
            {
                return true;
            }

            if (zStart >= zDestination && zCurrent <= zDestination)
            {
                return true;
            }

            return false;
        }
    }
}