using System.Collections.Generic;
using NearClientUnity.Utilities;

namespace Near.Models
{
    public class MarketplaceToken
    {
        public int Id { get; set; }
        public UInt128 Price { get; set; }
        public Token Token { get; set; }
        public bool isAuction { get; set; }
        public List<Offer> Offers { get; set; }
    }
}