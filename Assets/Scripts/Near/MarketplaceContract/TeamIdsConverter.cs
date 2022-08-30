using System;
using System.Collections.Generic;
using System.Linq;
using Near.Models.Game.TeamIds;
using Near.Models.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

namespace Near.MarketplaceContract
{
    public class TeamIdsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(TeamIds));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JObject.Load(reader);
            var teamIds = data["data"]?["team"];
            
            TeamIds result = new TeamIds();
            
            if (teamIds == null)
            {
                return result;
            }

            
            foreach (var five in teamIds["fives"].ToArray())
            {
                var newFive = new FiveIds();
                newFive.number = five["number"].ToString();
                newFive.ice_time_priority = five["ice_time_priority"].ToString();
                newFive.tactic = five["tactic"].ToString();
                foreach (var fieldPlayer in five["field_players"].ToArray())
                {
                    newFive.field_players.Add(fieldPlayer["position"].ToString(), fieldPlayer["token_id"].ToString());
                }
                result.fives.Add(newFive.number, newFive);
            }

            foreach (var goalie in teamIds["goalies"].ToArray())
            {
                result.goalies.Add(goalie["number"].ToString(), goalie["token_id"].ToString());
            }
            
            foreach (var goalie in teamIds["goalie_substitutions"].ToArray())
            {
                result.goalie_substitutions.Add(goalie["number"].ToString(), goalie["token_id"].ToString());
            }

            return result;
        }
    }
}
