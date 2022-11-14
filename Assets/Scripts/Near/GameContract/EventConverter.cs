using System;
using System.Collections.Generic;
using Near.Models.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Near.GameContract
{
    public class EventConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JObject.Load(reader);
            var events = data["data"]?["events"];

            List<Event> result = new List<Event>();

            if (events == null)
            {
                return result;
            }
            
            foreach (var jEvent in events)
            {
                var json = jEvent.ToString();
                var eventData = JsonConvert.DeserializeObject<Event>(json);
                result.Add(eventData);
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<Event>));
        }
    }
}