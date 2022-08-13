using System;
using System.Collections.Generic;
using Near.Models.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.GameContract
{
    public class UserGameContractConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JObject.Load(reader);
            var users = data["data"]?["GetUsers"];
            //var token = JsonConvert.DeserializeObject<Token>(json, new PlayerConverter());

            List<User> result = new List<User>();

            if (users == null)
            {
                return result;
            }
            
            foreach (var jUser in users)
            {
                var json = jUser.ToString();
                var user = JsonConvert.DeserializeObject<User>(json);
                result.Add(user);
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<User>));
        }
    }
}