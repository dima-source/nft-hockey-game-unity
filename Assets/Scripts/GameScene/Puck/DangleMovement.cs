using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public class DangleMovement : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly List<int> _randomNumbers;
        private readonly int _numberOfVectors;

        public DangleMovement(Vector3 startCoordinates, Vector3 destinationCoordinates, 
            List<int> randomNumbers, int numberOfVectors)
        {
            _startCoordinates = startCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _randomNumbers = randomNumbers;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            var convertedZ = TrajectoryUtils.GetConvertedZDestination(_startCoordinates, _destinationCoordinates);
            var step = (convertedZ - _startCoordinates.z) / _numberOfVectors;
            var currentCoordinates = _startCoordinates;
            
            var result = new List<Vector3>();

            for (int i = 0; i < _numberOfVectors; i++)
            {
                currentCoordinates.z += step;
                currentCoordinates.x = Mathf.Sin(currentCoordinates.z);
                
                result.Add(currentCoordinates);
            }

            result = TrajectoryUtils.RotateTrajectory(result, _destinationCoordinates);

            return result;
        }
        
    }
}