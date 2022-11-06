using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game.Team
{
    public class GoalieSubstitution
    {
        public string id { get; set; }
        public string substitution { get; set; }
        public FieldPlayer field_player { get; set; }
    }
}