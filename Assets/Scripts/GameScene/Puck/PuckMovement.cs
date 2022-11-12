using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = Near.Models.Game.Event;

namespace GameScene.Puck
{
    public class PuckMovement
    {
        private float _fieldLenght;
        private float _fieldWidth;
        
        public PuckMovement(float fieldLenght, float fieldWidth)
        {
            _fieldLenght = fieldLenght;
            _fieldWidth = fieldWidth;
        }
        
        public List<Vector3> GetTrajectory(Event eventData, Vector3 puckCoordinates)
        {
            var trajectories = new List<List<Vector3>>();

            Random.InitState(eventData.random_numbers[0]);
            foreach (var action in eventData.Actions)
            {
                var puckMovement = action.GetPuckMovement(eventData, puckCoordinates);
                var actionTrajectory = puckMovement.GetTrajectory();
                if (actionTrajectory.Count == 0) continue;
                trajectories.Add(actionTrajectory);
            }

            return trajectories.Last();
        }
    }
}