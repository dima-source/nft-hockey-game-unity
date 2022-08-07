using Near.Models.Extras;
using Newtonsoft.Json;

namespace Near.Models.Marketplace
{
    public class Goalie : Player
    {
        [JsonIgnore]
        public GoalieStats Stats { get; set; } 
        
        public override Token GetData()
        {
            return this;
        } 
    }
}