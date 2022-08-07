namespace Near.Models.Marketplace
{
    public class Offer
    {
        public int Id { get; set; }
        public string Price { get; set; } // UInt128
        public User User { get; set; }
    }
}