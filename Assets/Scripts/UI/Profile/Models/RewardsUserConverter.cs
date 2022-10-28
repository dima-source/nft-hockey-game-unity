using System;
using System.Collections.Generic;
using Near.MarketplaceContract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UI.Profile.Models
{
    public class RewardsUserConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(RewardsUser));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JObject.Load(reader);
            var user = data["data"]?["user"];

            if (user == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<RewardsUser>(user.ToString());
        }
    }
}