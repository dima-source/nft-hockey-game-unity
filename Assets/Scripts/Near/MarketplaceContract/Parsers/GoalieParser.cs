using System;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.Goalie;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public class GoalieParser : IPlayerParser
    {
        public NFT ParsePlayer(JObject data)
        {
            Goalie goalie = JsonConvert.DeserializeObject<Goalie>(data.ToString());
            if (goalie == null)
            {
                throw new Exception("Cannot deserialize goalie");
            }
            
            goalie.Stats = JsonConvert.DeserializeObject<GoalieStats>(data["stats"].ToString());

            return goalie;
        }
    }
}