using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game
{
    public class PlayerOnPosition
    {
        public int id { get; set; }
        public FieldPlayer Player{get; set; }
        public string position { get; set; }
    }
}