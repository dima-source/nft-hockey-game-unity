using System.Collections.Generic;
using System.Linq;
using GraphQL.Query.Builder;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;

namespace Near.Models.Game.Team
{
    public class Team
    {
        public string id { get; set; }
        public List<Five> fives { get; set; }
        public ActiveFive active_five { get; set; }
        public List<FieldPlayer> penalty_players { get; set; }
        public List<FieldPlayer> players_to_small_penalty { get; set; }
        public List<FieldPlayer> players_to_big_penalty { get; set; }
        public List<GoalieSubstitution> goalie_substitutions { get; set; }
        public string active_goalie_substitution { get; set; }
        public List<Goalie> goalies { get; set; }
        public string active_goalie { get; set; }
        public int score { get; set; }

        public static IQuery<Team> GetQuery(IQuery<Team> query)
        {
            query.AddField(t => t.id)
                .AddField(t => t.fives, Five.GetQuery)
                .AddField(t => t.active_five, ActiveFive.GetQuery)
                .AddField(t => t.penalty_players, FieldPlayer.GetQuery)
                .AddField(t => t.players_to_big_penalty, FieldPlayer.GetQuery)
                .AddField(t => t.players_to_small_penalty, FieldPlayer.GetQuery)
                .AddField(t => t.goalie_substitutions, GoalieSubstitution.GetQuery)
                .AddField(t => t.active_goalie_substitution)
                .AddField(t => t.goalies, Goalie.GetQuery)
                .AddField(t => t.active_goalie)
                .AddField(t => t.score);
            
            return query;
        }
    }
}