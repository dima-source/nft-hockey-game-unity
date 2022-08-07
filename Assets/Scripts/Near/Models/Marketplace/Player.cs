using System;
using Newtonsoft.Json;

namespace Near.Models.Marketplace
{
    public abstract class Player : Token
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