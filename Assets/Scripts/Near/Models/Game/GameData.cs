using System.Collections.Generic;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game
{
    public class GameData
    {
        public int ID { get; set; }
        public UserInGameInfo user1 { get; set; }
        public UserInGameInfo user2 { get; set; }        
        public int reward { get; set; }
        public int winner_index { get; set; }
        public int  last_event_generation_time { get; set; }
        public int turns { get; set; }
        public int zone_number { get; set; }
        public List<Event> events { get; set; } 
        public FieldPlayer player_with_puck { get; set; }
    }
}