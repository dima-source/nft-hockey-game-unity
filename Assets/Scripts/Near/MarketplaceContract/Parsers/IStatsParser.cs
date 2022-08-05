using Near.Models;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public interface IStatsParser
    {
        public Stats ParseExtra(JObject data);
    }
}