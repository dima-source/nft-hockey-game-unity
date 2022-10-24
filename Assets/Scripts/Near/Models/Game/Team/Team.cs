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

        public PlayerOnPosition GetPlayerOnPositionInActiveFiveById(string playerId)
        {
            var activeFive = GetActiveFive();
            return activeFive.GetPlayerOnPositionById(playerId);
        }

        public PlayerOnPosition GetPlayerOnPositionInActiveFiveByPosition(string position)
        {
            var activeFive = GetActiveFive();
            return activeFive.GetPlayerOnPositionByPosition(position);
        }

        private Five GetActiveFive()
        {
            return fives.Find(five => five.number == active_five);
        }
    }
}