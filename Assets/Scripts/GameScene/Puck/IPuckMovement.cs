using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Puck
{
    public interface IPuckMovement
    {
        List<Vector3> GetTrajectory();
    }
}