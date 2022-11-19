using System.Threading.Tasks;
using UnityEngine;

namespace GameScene.Puck
{
    public class Puck : MonoBehaviour
    {
        [SerializeField] private PuckTrack puckTrack;

        public async Task Move(Trajectory trajectory)
        {
            var delay = trajectory.RenderingSpeed / trajectory.NumberOfPoints;
            
            foreach (var coordinates in trajectory.Coordinates)
            {
                puckTrack.DrawTrack(transform.position);

                /*
                var delay = CalculateDelay(transform.position, coordinates,
                    trajectory.RenderingSpeed, trajectoryLenght);
                */
                transform.position = coordinates;
                
                await Task.Delay(delay);
            }
            
            puckTrack.ClearTrack();
        }
    }
}