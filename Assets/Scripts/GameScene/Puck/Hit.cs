using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Puck
{
    public class Hit : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _moveDestinationCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly int _numberOfVectors;

        public Hit(Vector3 startCoordinates, Vector3 moveDestinationCoordinates,
            Vector3 destinationCoordinates, int numberOfVectors)
        {
            _startCoordinates = startCoordinates;
            _moveDestinationCoordinates = moveDestinationCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            var moveMovement = new MoveMovement(_startCoordinates, _moveDestinationCoordinates, _numberOfVectors);
            var moveTrajectory = moveMovement.GetTrajectory();

            var result = moveTrajectory.SkipLast(_numberOfVectors / 200).ToList();

            var shotMovement = new ShotMovement(new List<Vector3> {_destinationCoordinates}, 
                result.Last(), _numberOfVectors, 0.003f);
            var shotTrajectory = shotMovement.GetTrajectory();
            result.AddRange(shotTrajectory);

            return result;
        }
    }
}