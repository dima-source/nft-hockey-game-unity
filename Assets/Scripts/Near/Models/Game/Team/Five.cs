using System.Collections.Generic;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game.Team
{
    public class Five
    {
        public int id { get; set; }
        public Dictionary<string, FieldPlayer> FieldPlayers;
        public string Number;
        public string IceTimePriority;
        public int time_field { get; set; }
        public string tactic { get; set; }
       
    }
}