using NearClientUnity.Utilities;

namespace Near.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public UInt128 Price { get; set; }
        public User User { get; set; }
    }
}