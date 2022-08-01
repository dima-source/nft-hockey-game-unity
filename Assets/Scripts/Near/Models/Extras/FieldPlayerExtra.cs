using Newtonsoft.Json;

namespace Near.Models.Extras
{
    public class FieldPlayerExtra : PlayerExtra
    {
        [JsonProperty("stats")]
        public FieldPlayerStats Stats { get; set; }
        
        public override Extra GetExtra()
        {
            return this;
        }
    }
}