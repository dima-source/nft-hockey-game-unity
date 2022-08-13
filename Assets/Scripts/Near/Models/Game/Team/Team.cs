using System.Collections.Generic;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;

namespace Near.Models.Game.Team
{
    public class Team
    {
        public int id { get; set; }
        public Dictionary<string,Five> Fives;
        public Dictionary<string,Goalie> Goalies;
        public int score { get; set; }
        public List<FieldPlayer> FieldPlayers;
        public string active_five { get; set; }
        public string active_goalie { get; set; }
        
    }
}