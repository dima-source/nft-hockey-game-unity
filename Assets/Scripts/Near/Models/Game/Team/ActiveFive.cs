using System.Collections.Generic;

namespace Near.Models.Game.Team
{
    public class ActiveFive
    {
        public string id { get; set; }
        public string current_number { get; set; }
        public List<string> replaced_position { get; set; }
        public List<PlayerOnPosition> field_players { get; set; }
        public bool is_goalie_out { get; set; }
        public string ice_time_priority { get; set; }
        public string tactic { get; set; }
        public int time_field { get; set; }
    }
}