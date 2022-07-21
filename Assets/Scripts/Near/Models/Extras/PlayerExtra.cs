using Newtonsoft.Json;

namespace Near.Models.Extras
{
    public abstract class PlayerExtra : Extra
    {
        [JsonProperty("reality")]
        public bool Reality { get; set; }
        
        [JsonProperty("nationality")]
        public string Nationality { get; set; }
        
        [JsonProperty("birthday")]
        public ulong Birthday { get; set; }

        [JsonProperty("number")]
        public uint Number { get; set; }
        
        [JsonProperty("hand")]
        public string Hand { get; set; }
        
        [JsonProperty("player_role")]
        public string PlayerRole { get; set; }
        
        [JsonProperty("player_position")]
        public string PlayerPosition { get; set; }
    }
}