using System.Collections.Generic;
using GraphQL.Query.Builder;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;

namespace Near.Models.Game.Team
{
    public class Five
    {
        public string id { get; set; }
        public List<PlayerOnPosition> field_players { get; set; }
        public string number { get; set; }
        public string ice_time_priority { get; set; }
        public int time_field { get; set; }
        public string tactic { get; set; }
        
         public static IQuery<Five> GetQuery(IQuery<Five> query)
         {
             query.AddField(f => f.id)
                 .AddField(f => f.field_players, PlayerOnPosition.GetQuery)
                 .AddField(f => f.number)
                 .AddField(f => f.ice_time_priority)
                 .AddField(f => f.tactic)
                 .AddField(f => f.time_field);
            
            return query;
        }
    }
}