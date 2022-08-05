using Near.Models;
using Near.Models.Extras;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public class GoalieStatsParser : IStatsParser
    {
        public Stats ParseExtra(JObject data)
        {
            return JsonConvert.DeserializeObject<GoalieStats>(data.ToString());
        }
    }
}