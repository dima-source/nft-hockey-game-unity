using Newtonsoft.Json;

namespace Near.Models.Extras
{
    public class GoalieExtra : PlayerExtra
    {
        [JsonProperty("stats")]
        public GoalieStats Stats { get; set; }
        
        public override Extra GetExtra()
        {
            return this;
        }
    }
}