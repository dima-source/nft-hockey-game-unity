using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GameScene.Puck
{
    public class Puck : MonoBehaviour
    {
        [SerializeField] private PuckTrack puckTrack;

        public async Task Move(Trajectory trajectory)
        {
            var trajectoryLenght = trajectory.GetTrajectoryLenght();
            var delay = (float)trajectory.RenderingSpeed / trajectory.Coordinates.Count;
            foreach (var coordinates in trajectory.Coordinates)
            {
                puckTrack.DrawTrack(transform.position);

                /*
                var delay = CalculateDelay(transform.position, coordinates,
                    trajectory.RenderingSpeed, trajectoryLenght);
                */
                transform.position = coordinates;
                
                await Task.Delay((int)delay);
            }
            
            puckTrack.ClearTrack();
        }

        private int CalculateDelay(Vector3 start, Vector3 destination, int renderingSpeed, float trajectoryLenght)
        {
            var xSide = Mathf.Pow(start.x - destination.x, 2);
            var zSide = Mathf.Pow(start.z - destination.z, 2);
            var distance = Mathf.Sqrt(xSide + zSide);

            var dTrajectory = distance / trajectoryLenght;

            return (int)(dTrajectory * renderingSpeed);
        }
    }
}