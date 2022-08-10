using System.Collections.Generic;

namespace Near.Models.Tokens
{
    public class User
    {
        public string id { get; set; }
        public List<string> tokens { get; set; }
    }
}