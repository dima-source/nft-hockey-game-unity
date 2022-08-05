using System;
using NearClientUnity.Utilities;

namespace Near.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Media { get; set; }
        public string Extra { get; set; }
        public bool Reality { get; set; }
        public string Stats { get; set; }
        public int Number { get; set; }
        public string Hand { get; set; }
        public string Player_role { get; set; }
        public string Native_position { get; set; }
        public string Player_type { get; set; }
        public string Rarity { get; set; }
        public DateTime Birthday { get; set; }
        public UInt128 Issued_at { get; set; }
        public string TokenId { get; set; }
        public User Owner { get; set; }
        public string OwnerId { get; set; }
        public string Perpetual_royalties { get; set; }
        public MarketplaceToken MarketplaceData { get; set; }
    }
}