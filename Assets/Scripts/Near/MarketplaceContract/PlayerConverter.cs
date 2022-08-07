using System;
using Newtonsoft.Json;
using Near.MarketplaceContract.Parsers;
using Near.Models;
using Near.Models.Marketplace;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract
{
    public class PlayerConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Token));
        }

        private IPlayerParser MapDataToParser(JObject data)
        {
            string type = data["player_type"].ToString();
            
            return type switch
            {
                "Goalie" => new GoalieParser(),
                "FieldPlayer" => new FieldPlayerParser(),
                _ => throw new Exception("Extra type not found")
            };
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var player = JObject.Load(reader);
            IPlayerParser parser = MapDataToParser(player);
            return parser.ParsePlayer(player);
        }
    }
}