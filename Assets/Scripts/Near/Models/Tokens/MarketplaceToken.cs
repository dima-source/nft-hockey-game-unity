using System.Collections.Generic;
using Newtonsoft.Json;

namespace Near.Models.Tokens
{
    public class MarketplaceToken
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("price")]
        public string Price { get; set; } // UInt128
        
        [JsonProperty("token")]
        public NFT NFT { get; set; }
        
        [JsonProperty("isAuction")]
        public bool IsAuction { get; set; }
        
        [JsonProperty("offers")]
        public List<Offer> Offers { get; set; }
    }
}