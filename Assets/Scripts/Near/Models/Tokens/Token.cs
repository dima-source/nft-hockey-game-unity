using Near.Models.Game;

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
        
        
        /// <param name="statsAvg">The indicator by which the rarity of the token is determined</param>
        public Rarity GetRarity(float statsAvg)
        {
            if (statsAvg >= 95)
            {
                return Rarity.Exclusive;
            }
            
            if (statsAvg >= 85)
            {
                return Rarity.Unique;
            }

            if (statsAvg >= 75)
            {
                return Rarity.Rare;
            }

            if (statsAvg >= 60)
            {
                return Rarity.Uncommon;
            }

            return Rarity.Common;
        }
    }
}