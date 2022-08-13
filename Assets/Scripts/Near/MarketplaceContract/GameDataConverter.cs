using System;
using System.Collections.Generic;
using Near.Models.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.MarketplaceContract.Parsers
{
    public class GameDataConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JObject.Load(reader);
            var gameDatas = data["data"]?["GetData"];
            //var token = JsonConvert.DeserializeObject<Token>(json, new PlayerConverter());

            List<GameData> result = new List<GameData>();

            if (gameDatas == null)
            {
                return result;
            }
            
            foreach (var jGameData in gameDatas)
            {
                var json = jGameData.ToString();
                var gameData = JsonConvert.DeserializeObject<GameData>(json);
                result.Add(gameData);
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<GameData>));
        }
    }
}