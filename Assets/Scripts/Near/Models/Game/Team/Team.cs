using System.Collections.Generic;
using Near.Models.Tokens.Players.Goalie;

namespace Near.Models.Game.Team
{
    public class Team
    {
        public Dictionary<string, Five> Fives;
        public Dictionary<string, Goalie> Goalies;
    }
}