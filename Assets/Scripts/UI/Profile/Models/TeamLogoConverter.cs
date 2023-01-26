using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace UI.Profile.Models
{
    public class TeamLogoConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(TeamLogo));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JObject.Load(reader);
            var user = data["data"]?["teamLogo"];
            
            if (user == null || !user.Any())
            {
                return new TeamLogo();
            }

            return JsonConvert.DeserializeObject<TeamLogo>(user.ToString());
        }
    }
}