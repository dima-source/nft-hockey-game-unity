using Near.Models;
using Near.Models.Extras;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public class FieldPlayerStatsParser : IStatsParser
    {
        public Stats ParseStats(JObject data)
        {
            return JsonConvert.DeserializeObject<FieldPlayerStats>(data.ToString());
        }

        public Stats ParseExtra(JObject data)
        {
            throw new System.NotImplementedException();
        }
    }
}