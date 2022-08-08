using Newtonsoft.Json;

namespace Near.Models.Tokens
{
    public class Offer
    {
        [JsonProperty("price")]
        public string Price { get; set; } // UInt128
        
        [JsonProperty("user")]
        public User User { get; set; }
    }
}