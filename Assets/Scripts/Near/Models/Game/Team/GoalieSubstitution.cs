using GraphQL.Query.Builder;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game.Team
{
    public class GoalieSubstitution
    {
        public string id { get; set; }
        public string substitution { get; set; }
        public FieldPlayer player { get; set; }

        public static IQuery<GoalieSubstitution> GetQuery(IQuery<GoalieSubstitution> query)
        {
            query.AddField(g => g.id)
                .AddField(g => g.substitution)
                .AddField(g => g.player, FieldPlayer.GetQuery);

            return query;
        }
 
    }
}