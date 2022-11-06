using System.Collections.Generic;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game
{
    public class GameData
    {
        public int id { get; set; }
        public UserInGameInfo user1 { get; set; }
        public UserInGameInfo user2 { get; set; }        
        public string reward { get; set; }
        public string winner_index { get; set; }
        public List<Event> events { get; set; } 
    }
}