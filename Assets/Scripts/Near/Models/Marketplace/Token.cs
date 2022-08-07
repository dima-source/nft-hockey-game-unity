using Newtonsoft.Json;

namespace Near.Models.Marketplace
{
    public abstract class Token
    {
        [JsonProperty("player_type")]
        public string TokeType { get; set; }
        
        [JsonProperty("title")]
        public string Name { get; set; }
        
        [JsonProperty("media")]
        public string Media { get; set; }
        
        [JsonProperty("rarity")]
        public string Rarity { get; set; }
        
        [JsonProperty("issued_at")]
        public string IssuedAt { get; set; } // UInt128
        
        [JsonProperty("tokenId")]
        public string TokenId { get; set; }
        
        [JsonProperty("owner")]
        public User Owner { get; set; }
        
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }
        
        [JsonProperty("perpetual_royalties")]
        public string PerpetualRoyalties { get; set; }
        
        [JsonProperty("marketplace_data")] 
        public MarketplaceToken MarketplaceData { get; set; }

        public abstract Token GetData();
    }
}