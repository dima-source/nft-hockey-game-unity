using System.Collections.Generic;

namespace Near.Models.Tokens.Filters
{
    public class PlayerFiler
    {
        public string title { get; set; }
        public string ownerId { get; set; }
        public string hand { get; set; }
        public string player_type { get; set; }
        public string birthday_gte { get; set; }
        public string birthdat_lte { get; set; }
        
        public List<string> rarity_in { get; set; }
        public List<string> player_role_in { get; set; }
        public List<string> nationality_in { get; set; }
    }
}