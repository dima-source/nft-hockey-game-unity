using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game.Team
{
    public class PlayerOnPosition
    {
        public string id { get; set; }
        public FieldPlayer player { get; set; }
        public string position { get; set; }
    }
}