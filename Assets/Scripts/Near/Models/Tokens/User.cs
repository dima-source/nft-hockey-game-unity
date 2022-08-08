using System.Collections.Generic;

namespace Near.Models.Tokens
{
    public class User
    {
        public string Id { get; set; }
        public List<NFT> Tokens { get; set; }
    }
}