using Newtonsoft.Json;

namespace Near.Models.Tokens.Players.FieldPlayer
{
    public class FieldPlayer : Player
    {
        [JsonIgnore]
        public FieldPlayerStats Stats { get; set; }
    }
}