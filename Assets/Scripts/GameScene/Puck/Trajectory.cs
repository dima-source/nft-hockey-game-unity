using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public struct Trajectory
    {
        public List<Vector3> Coordinates { get; set; }
        public int RenderingSpeed { get; set; }

        public float GetTrajectoryLenght()
        {
            if (Coordinates == null || Coordinates.Count == 0) return 0;

            float result = 0;
            var currentCoordinates = Coordinates[0];
            
            for (int i = 1; i < Coordinates.Count; i++)
            {
                var xSide = Mathf.Pow(currentCoordinates.x - Coordinates[i].x, 2);
                var zSide = Mathf.Pow(currentCoordinates.z - Coordinates[i].z, 2);
                var distance = Mathf.Sqrt(xSide + zSide);

                currentCoordinates = Coordinates[i];
                
                result += distance;
            }

            return result;
        }
    }
}