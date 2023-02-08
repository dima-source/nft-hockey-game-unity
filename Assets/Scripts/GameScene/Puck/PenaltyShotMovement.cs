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

        public PenaltyShotMovement(Vector3 startCoordinates, Vector3 destinationCoordinates, int numberOfVectors = 1000)
        {
            _startCoordinates = startCoordinates;
            _destinationCoordinates = destinationCoordinates;
            _numberOfVectors = numberOfVectors;
        }
        
        public List<Vector3> GetTrajectory()
        {
            var convertedZ = TrajectoryUtils.GetConvertedZDestination(_startCoordinates, _destinationCoordinates);
            var absConvertedZ = Mathf.Abs(convertedZ);
            var currentCoordinates = _startCoordinates;

            var partMove = 5 / 6.0f;
            var moveConvertedZ = absConvertedZ * partMove;
            var moveNumberOfVectors = _numberOfVectors * partMove;
            var moveStep = moveConvertedZ / moveNumberOfVectors;
            var moveSpline = GetMoveSpline(moveConvertedZ);
            var isMirrorMove = TrajectoryUtils.IsProbabilityInRandomRange(50);
            
            var moveZ = convertedZ * partMove;
            var moveX = Random.Range(-2, 2);
            var moveDestination = new Vector3(moveX, _destinationCoordinates.y, moveZ);    
            
            var result = GetRelativeCoordinates(
                currentCoordinates, moveSpline, (int)moveNumberOfVectors, isMirrorMove, moveStep);

            result = TrajectoryUtils.RotateTrajectory(result, moveDestination);
            currentCoordinates = result.Last();
            
            var shotNumberOfVectors = _numberOfVectors;
            var shotMovement = new ShotMovement(
                    new List<Vector3> {_destinationCoordinates}, currentCoordinates, shotNumberOfVectors);
            
            result.AddRange(shotMovement.GetTrajectory());
        
            return result;
        }

        private CubicSpline GetMoveSpline(float convertedZ)
        {
            var numberOfSplinePoints = 8;
            var numberOfMovePoints = 3;
            var stepSpline = convertedZ / numberOfSplinePoints;

            var zSpline = new float[numberOfSplinePoints];
            var xSpline = new float[numberOfSplinePoints];

            zSpline[0] = _startCoordinates.z;
            xSpline[0] = _startCoordinates.x;
            
            for (int i = 1; i < numberOfSplinePoints; i++)
            {
                zSpline[i] = zSpline[i-1] + stepSpline;

                var rndSign = TrajectoryUtils.IsProbabilityInRandomRange(50) ? -1 : 1;
                if (i < numberOfMovePoints)
                {
                    var rndX = Random.Range(3f, 6f);
                    xSpline[i] = rndX * rndSign;
                }
                else
                {
                    var rndX = Random.Range(1f, 2f);
                    xSpline[i] = rndX * rndSign;
                }
            }

            CubicSpline spline = new CubicSpline();
            spline.BuildSpline(zSpline, xSpline, numberOfSplinePoints);

            return spline;
        }
        
        private List<Vector3> GetRelativeCoordinates(Vector3 currentCoordinates, CubicSpline spline, int numberOfVectors, bool isMirror, float step)
        {
            var result = new List<Vector3>();
            for (int i = 0; i < numberOfVectors; i++)
            {
                currentCoordinates.z += step;
                currentCoordinates.x = (float) spline.Interpolate(currentCoordinates.z);
                
                if (isMirror)
                {
                    currentCoordinates.x = TrajectoryUtils.MirrorCoordinate(
                        _startCoordinates.x, currentCoordinates.x);
                }
                
                result.Add(currentCoordinates);
            }

            return result;
        }
    }
}