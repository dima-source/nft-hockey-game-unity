using Newtonsoft.Json;

namespace Near.Models.Tokens.Players
{
    public abstract class Player : Token
    {
        public int id { get; set; }
        public string IMG { get; set; }
        public string Name { get; set; }
        public string nationality { get; set; }
        public bool reality { get; set; }
        public int number { get; set; }
        public string hand { get; set; }
        public string player_role { get; set; }
        public string native_position { get; set; }
        public string birthday { get; set; } // timestamp
        
        [JsonIgnore]
        public string stats { get; set; }
    }
}