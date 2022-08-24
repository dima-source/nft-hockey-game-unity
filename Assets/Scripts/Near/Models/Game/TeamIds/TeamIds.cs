using System.Collections.Generic;

namespace Near.Models.Game.TeamIds
{
    public class TeamIds
    {
        public Dictionary<string, FiveIds> fives = new();
        public Dictionary<string, string> goalies = new();
        public Dictionary<string, string> goalie_substitutions = new();
    }
}