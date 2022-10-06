using System.Collections.Generic;

namespace Near.Models.Tokens.Filters
{
    public class PlayerFilter
    {
        public string title { get; set; }
        public string title_starts_with_nocase { get; set; }
        public string ownerId { get; set; }
        public string ownerId_not { get; set; }
        public long birthday_gte { get; set; }
        public long birthday_lte { get; set; }
        
        public List<string> rarity_in { get; set; }
        public List<string> native_position_in { get; set; }
        public List<string> player_type_in { get; set; }
        public List<string> hand_in { get; set; }
        public List<string> player_role_in { get; set; }
        public List<string> nationality_in { get; set; }
        
        public MarketplaceTokenFilter marketplace_data_ { get; set; }
    }
}