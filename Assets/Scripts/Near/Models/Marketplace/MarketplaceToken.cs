using System.Collections.Generic;
using Near.Models.Marketplace;
using NearClientUnity.Utilities;

namespace Near.Models
{
    public class MarketplaceToken
    {
        public int Id { get; set; }
        public string Price { get; set; } // UInt128
        public Token Token { get; set; }
        public bool isAuction { get; set; }
        public List<Offer> Offers { get; set; }
    }
}