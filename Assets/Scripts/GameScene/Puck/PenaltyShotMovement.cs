using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Puck
{
    public class PenaltyShotMovement : IPuckMovement
    {
        private readonly Vector3 _startCoordinates;
        private readonly Vector3 _destinationCoordinates;
        private readonly Vector3 _shotCoordinates;
        private readonly int _numberOfVectors;

        public PenaltyShotMovement(Vector3 startCoordinates, Vector3 shotCoordinates, Vector3 destinationCoordinates, int numberOfVectors)
        {
            _startCoordinates = startCoordinates;
            _shotCoordinates = shotCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            return new List<Vector3>();
        }
    }
}