using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public class DefaultMovement : IPuckMovement
    {
        public List<Vector3> GetTrajectory()
        {
            return new List<Vector3>();
        }
    }
}