namespace Near.Models.Tokens
{
    public class Offer
    {
        public string price { get; set; } // UInt128
        public User user { get; set; }
    }
}