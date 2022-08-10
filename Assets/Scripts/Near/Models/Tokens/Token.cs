namespace Near.Models.Tokens
{
    public abstract class Token
    {
        public string title { get; set; }
        public string player_type { get; set; }
        public string media { get; set; }
        public string rarity { get; set; }
        public string issued_at { get; set; } 
        public string tokenId { get; set; }
        public User owner { get; set; }
        public string ownerId { get; set; }
        public string perpetual_royalties { get; set; }
        public MarketplaceToken marketplace_data { get; set; }
    }
}