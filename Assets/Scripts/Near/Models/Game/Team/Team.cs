using System.Collections.Generic;
using GraphQL.Query.Builder;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;

namespace Near.Models.Game.Team
{
    public class Team
    {
        public int id { get; set; }
        public List<Five> Fives;
        public List<Goalie> Goalies;
        public int score { get; set; }
        public List<FieldPlayer> FieldPlayers;
        public string active_five { get; set; }
        public string active_goalie { get; set; }

        public static IQuery<Team> GetQuery(IQuery<Team> query)
        {
            // TODO
            return query;
        }
    }
}