using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Puck
{
    /// <summary>
    /// To build a trajectory, you need to call the GetConvertedZDestination method.
    /// Then build a horizontal trajectory.
    /// After that, call the RotateTrajectory method, which will rotate the trajectory 
    /// </summary>
    public static class TrajectoryUtils
    {
        /// <param name="start">Puck coordinates</param>
        /// <param name="destination">Coordinates where the puck should arrive</param>
        /// <returns>Transformed destination Z coordinate</returns>
        public static float GetConvertedZDestination(Vector3 start, Vector3 destination)
        {
            var distance = GetDistance(destination, start);

            if (destination.z < start.z)
            {
                return start.z - distance;
            }

            return distance + start.z;
        }
        
        /// <summary>
        /// Rotates the path so that the end point of this path is equal to the destination point
        /// Transforms in X and Z
        /// </summary>
        /// <param name="trajectory">The path to be turned</param>
        /// <param name="destination"></param>
        public static List<Vector3> RotateTrajectory(List<Vector3> trajectory, Vector3 destination)
        {
            if (trajectory == null || trajectory.Count == 0) return new List<Vector3>();

            var startPoint = trajectory[0];
            
            // Angle between start and destination coordinate and horizontal line
            var alpha1 = GetAngle(destination, startPoint);
            
            var dAlpha = GetAngle(trajectory.Last(), startPoint);
            
            // The angle by which the entire trajectory must be rotated
            var alpha = alpha1 - dAlpha;
            
            var result = new List<Vector3> {startPoint};
            for (int i = 1; i < trajectory.Count; i++)
            {
                var dAlphaPoint = GetAngle(trajectory[i], startPoint);
                var distance = GetDistance(trajectory[i], startPoint);

                var distance1 = Mathf.Cos(dAlphaPoint) * distance;
               
                var alphaPoint = alpha - dAlphaPoint;
                var angle = GetConvertedAngle(alphaPoint, destination, startPoint);
                var x = Mathf.Sin(angle) * distance1 + startPoint.x;
                var z = Mathf.Cos(angle) * distance1 + startPoint.z;

                var convertedPoint = new Vector3(x, trajectory[i].y, z);

                result.Add(convertedPoint);
            }
            
            return result;
        }

        private static float GetAngle(Vector3 destination, Vector3 start)
        {
            var xH = destination.x - start.x;
            var zH = destination.z - start.z;

            var tan = xH / zH;

            var result = Mathf.Atan(tan);

            return result;
        }

        private static float GetConvertedAngle(float angle, Vector3 destination, Vector3 start)
        {
            if (destination.z < start.z && destination.x > start.x || destination.z < start.z && destination.x < start.x)
            {
                return angle + Mathf.PI;
            }

            return angle;
        }
        private static float GetDistance(Vector3 destination, Vector3 start)
        {
            var xSide = Mathf.Pow(destination.x - start.x, 2);
            var zSide = Mathf.Pow(destination.z - start.z, 2);

            return Mathf.Sqrt(xSide + zSide);
        }
    }
}