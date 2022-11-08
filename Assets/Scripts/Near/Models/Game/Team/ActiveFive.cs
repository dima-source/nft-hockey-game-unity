using System.Collections.Generic;
using GraphQL.Query.Builder;

namespace Near.Models.Game.Team
{
    public class ActiveFive
    {
        public string id { get; set; }
        public string current_number { get; set; }
        public List<string> replaced_position { get; set; }
        public List<PlayerOnPosition> field_players { get; set; }
        public bool is_goalie_out { get; set; }
        public string ice_time_priority { get; set; }
        public string tactic { get; set; }
        public int time_field { get; set; }

        public static IQuery<ActiveFive> GetQuery(IQuery<ActiveFive> query)
        {
            query.AddField(f => f.id)
                .AddField(f => f.current_number)
                .AddField(f => f.replaced_position)
                .AddField(f => f.field_players, PlayerOnPosition.GetQuery)
                .AddField(f => f.is_goalie_out)
                .AddField(f => f.ice_time_priority)
                .AddField(f => f.tactic)
                .AddField(f => f.time_field);

            return query;
        }
 
    }
}