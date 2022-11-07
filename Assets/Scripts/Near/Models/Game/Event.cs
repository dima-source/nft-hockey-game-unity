using System;
using System.Collections.Generic;
using GraphQL.Query.Builder;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players.FieldPlayer;
using Newtonsoft.Json;
using Action = Near.Models.Game.Actions.Action;

namespace Near.Models.Game
{
    public class Event
    {
        public string id { get; set; }
        public int game_id { get; set; }
        public int event_number { get; set; }
        public UserInGameInfo user1 { get; set; }
        public UserInGameInfo user2 { get; set; }
        public FieldPlayer player_with_puck { get; set; }
        public int zone_number { get; set; }
        public string time { get; set; }
        public string event_generation_delay { get; set; }
        public List<string> actions { get; set; }
        
        [JsonIgnore]
        public List<Action> Actions { get; set; }
        

        public static IQuery<Event> GetQuery(IQuery<Event> query)
        {
            query.AddField(e => e.id)
                .AddField(e => e.game_id)
                .AddField(e => e.event_number)
                .AddField(e => e.user1, UserInGameInfo.GetQuery)
                .AddField(e => e.user2, UserInGameInfo.GetQuery)
                .AddField(e => e.player_with_puck, FieldPlayer.GetQuery)
                .AddField(e => e.actions)
                .AddField(e => e.zone_number)
                .AddField(e => e.time)
                .AddField(e => e.event_generation_delay);
            
            return query;
        }
    }
}