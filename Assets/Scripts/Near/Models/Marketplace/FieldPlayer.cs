using Near.Models.Extras;
using Newtonsoft.Json;

namespace Near.Models.Marketplace
{
    public class FieldPlayer : Player
    {
        [JsonIgnore]
        public FieldPlayerStats Stats { get; set; }

        public override Token GetData()
        {
            return this;
        }
    }
}