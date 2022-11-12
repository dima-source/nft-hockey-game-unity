using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public class MoveMovement : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly int _numberOfVectors;

        public MoveMovement(Vector3 startCoordinates, Vector3 destinationCoordinates, int numberOfVectors)
        {
            _startCoordinates = startCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            var convertedZ = TrajectoryUtils.GetConvertedZDestination(_startCoordinates, _destinationCoordinates);
            var distance = convertedZ - _startCoordinates.z;
            var step = (distance) / _numberOfVectors;
            
            var result = new List<Vector3>();
            var currentCoordinates = _startCoordinates;

            var rndZ = Random.Range(0.5f, 0.8f);
            var rndY = Random.Range(0.75f, 2f);
            var shiftPhase = TrajectoryUtils.GetRandomShiftPhase();
            
            for (int i = 0; i < _numberOfVectors; i++)
            {
                currentCoordinates.z += step;
                currentCoordinates.x = Mathf.Sin(rndZ * currentCoordinates.z + shiftPhase)*rndY;

                result.Add(currentCoordinates);
            }

            result = TrajectoryUtils.RotateTrajectory(result, _destinationCoordinates);

            return result;
        }
    }
}