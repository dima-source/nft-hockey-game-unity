using Near.Models;
using Near.Models.Extras;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public class GoalieExtraParser : IExtraParser
    {
        public Extra ParseExtra(JObject data)
        {
            return JsonConvert.DeserializeObject<GoalieExtra>(data.ToString());
        }
    }
}