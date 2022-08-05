using System.Collections.Generic;

namespace Near.Models
{
    public class User
    {
        public int Id { get; set; }
        public List<Token> Tokens { get; set; }
    }
}