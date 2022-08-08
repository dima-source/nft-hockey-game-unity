using Newtonsoft.Json;

namespace Near.Models.Tokens.Players
{
    public abstract class Player : NFT
    {
        [JsonProperty("reality")]
        public bool Reality { get; set; }
        
        [JsonProperty("number")]
        public int Number { get; set; }
        
        [JsonProperty("hand")]
        public string Hand { get; set; }
        
        [JsonProperty("player_role")]
        public string PlayerRole { get; set; }
        
        [JsonProperty("native_position")]
        public string NativePosition { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; } // timestamp
    }
}