using System.Collections.Generic;

namespace Near.Models.Tokens
{
    public class MarketplaceToken
    {
        public int id { get; set; }
        public string price { get; set; } // UInt128
        
        public Token token { get; set; }
        public bool isAuction { get; set; }
        public List<Offer> offers { get; set; }
    }
}