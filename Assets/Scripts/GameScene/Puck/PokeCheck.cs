using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Puck
{
    public class PokeCheck : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _dangleDestinationCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly int _numberOfVectors;

        public PokeCheck(Vector3 startCoordinates, Vector3 dangleDestinationCoordinates,
            Vector3 destinationCoordinates, int numberOfVectors)
        {
            _startCoordinates = startCoordinates;
            _dangleDestinationCoordinates = dangleDestinationCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            var dangleMovement = new DangleMovement(_startCoordinates, _dangleDestinationCoordinates, _numberOfVectors);
            var dangleTrajectory = dangleMovement.GetTrajectory();

            var result = dangleTrajectory.SkipLast(_numberOfVectors / 3).ToList();

            var shotMovement = new ShotMovement(new List<Vector3> {_destinationCoordinates},
                result.Last(), _numberOfVectors, 0.005f);
            var shotTrajectory = shotMovement.GetTrajectory();
            result.AddRange(shotTrajectory);

            return result;
        }
    }
}