using System.Collections.Generic;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game.Team
{
    public class Five
    {
        public int id { get; set; }
        public List<PlayerOnPosition> field_players;
        public string number { get; set; }
        public string ice_time_priority { get; set; }
        public int time_field { get; set; }
        public string tactic { get; set; }
    }
}