using Near.Models.Tokens;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public interface IPlayerParser
    {
        public NFT ParsePlayer(JObject data);
    }
}