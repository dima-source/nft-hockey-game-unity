using System;
using Newtonsoft.Json;
using Near.MarketplaceContract.Parsers;
using Near.Models;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract
{
    public class StatsConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Stats));
        }

        private IStatsParser MapDataToParser(JObject data)
        {
            string type = data["type"] != null ? data["type"].ToString() : data["player_type"].ToString();
            
            return type switch
            {
                "Goalie" => new GoalieStatsParser(),
                "FieldPlayer" => new FieldPlayerStatsParser(),
                _ => throw new Exception("Extra type not found")
            };
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stats = JObject.Load(reader);
            IStatsParser parser = MapDataToParser(stats);
            return parser.ParseExtra(stats);
        }
    }
}