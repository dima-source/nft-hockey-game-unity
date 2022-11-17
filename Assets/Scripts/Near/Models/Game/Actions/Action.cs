using System.Collections.Generic;
using GameScene.Puck;
using Newtonsoft.Json;
using UnityEngine;

namespace Near.Models.Game.Actions
{
    public abstract class Action
    {
        public string action_type { get; set; }

        // Color for messages
        protected readonly string UserColor = "<color=#0968dc>";
        protected readonly string OpponentColor = "<color=#bb461b>";
        protected readonly string DefaultColor = "<color=#6f6967>";
        
        public abstract string GetMessage(string accountId);
        
        public virtual IPuckMovement GetPuckMovement(Event eventData, Vector3 puckPosition)
        {
            return new DefaultMovement();
        }
    }
}