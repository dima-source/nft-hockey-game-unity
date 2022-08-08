using System;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public class FieldPlayerParser : IPlayerParser
    {
        public NFT ParsePlayer(JObject data)
        {
            FieldPlayer fieldPlayer = JsonConvert.DeserializeObject<FieldPlayer>(data.ToString());
            if (fieldPlayer == null)
            {
                throw new Exception("Cannot deserialize field player");
            }
            
            fieldPlayer.Stats = JsonConvert.DeserializeObject<FieldPlayerStats>(data["stats"].ToString());

            return fieldPlayer; 
        }
    }
}