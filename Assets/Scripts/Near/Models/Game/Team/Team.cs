using System.Collections.Generic;
using GraphQL.Query.Builder;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;

namespace Near.Models.Game.Team
{
    public class Team
    {
        public int id { get; set; }
        public List<Five> fives { get; set; }
        public string active_five { get; set; }
        public List<FieldPlayer> penalty_players { get; set; }
        public List<Goalie> goalies { get; set; }
        public string active_goalie { get; set; }
        public int score { get; set; }

        public static IQuery<Team> GetQuery(IQuery<Team> query)
        {
            query.AddField(t => t.id)
                .AddField(t => t.fives, Five.GetQuery)
                .AddField(t => t.active_five)
                .AddField(t => t.penalty_players, FieldPlayer.GetQuery)
                .AddField(t => t.goalies, Goalie.GetQuery)
                .AddField(t => t.active_goalie)
                .AddField(t => t.score);
            
            return query;
        }
    }
}