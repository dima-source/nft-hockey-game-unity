﻿using System.Collections.Generic;
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
            var relativeAngle = GetRelativeAngle(destination, startPoint);
            
            // The angle by which the entire trajectory must be rotated
            var alpha1 = GetAngle(trajectory.Last(), startPoint);
            var alpha = GetAngle(destination, startPoint) - relativeAngle + alpha1; 
            
            var result = new List<Vector3> {startPoint};
            for (int i = 1; i < trajectory.Count; i++)
            {
                var dAlphaPoint = GetAngle(trajectory[i], startPoint);
                var distance = GetDistance(trajectory[i], startPoint);

                var distance1 = Mathf.Cos(dAlphaPoint) * distance;
               
                var alphaPoint = alpha - dAlphaPoint;
                var x = Mathf.Sin(alphaPoint) * distance1 + startPoint.x;
                var z = Mathf.Cos(alphaPoint) * distance1 + startPoint.z;

                var convertedPoint = new Vector3(x, trajectory[i].y, z);

                result.Add(convertedPoint);
            }
            
            return result;
        }

        public static float GetAngle(Vector3 destination, Vector3 start)
        {
            var xH = destination.x - start.x;
            var zH = destination.z - start.z;

            var tan = xH / zH;

            var result = Mathf.Atan(tan);

            return result;
        }

        public static float GetRelativeAngle(Vector3 destination, Vector3 start)
        {
            if (destination.z < start.z && destination.x > start.x || destination.z < start.z && destination.x < start.x)
            {
                return Mathf.PI;
            }

            return 0;
        }
        
        public static float GetDistance(Vector3 destination, Vector3 start)
        {
            var xSide = Mathf.Pow(destination.x - start.x, 2);
            var zSide = Mathf.Pow(destination.z - start.z, 2);

            return Mathf.Sqrt(xSide + zSide);
        }

        public static float GetZLenght(List<Vector3> coordinates, Vector3 startPoint)
        {
            if (coordinates == null || coordinates.Count == 0) return 0;

            var currentCoordinates = startPoint; 
            float result = 0;

            for (int i = 0; i < coordinates.Count; i++)
            {
                result += Mathf.Abs(currentCoordinates.z - coordinates[i].z);
                currentCoordinates = coordinates[i];
            }

            return result;
        }
        
        public static Vector3 GetRandomPoint(Vector3 destination, Vector3 start)
        {
            var xRnd = Random.Range(start.x -1f, start.x + 1f);
            
            float zRnd;
            if (destination.z > start.z)
            {
                zRnd = Random.Range(start.z, destination.z);
            }
            else
            {
                zRnd = Random.Range(destination.z, start.z);
            }

            return new Vector3(xRnd, destination.y, zRnd);
        }

        public static float GetRandomShiftPhase()
        {
            return Random.Range(-Mathf.PI, Mathf.PI);
        }
        
        public static bool IsProbabilityInRandomRange(float probability)
        {
            var rnd = Random.Range(0, 100);
            return probability > rnd;
        }

        public static float GetAcceleration(float s, float v, float t)
        {
            return 2 * (s - v * t) / Mathf.Pow(t, 2);
        }
        
        /// <summary>
        /// Mirrors the coordinate relative to the start
        /// </summary>
        public static float MirrorCoordinate(float startCoordinate, float coordinate)
        {
            float distance = startCoordinate - coordinate;
            return startCoordinate + distance;
        }
    }
}