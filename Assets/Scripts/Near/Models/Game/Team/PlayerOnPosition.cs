using GraphQL.Query.Builder;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game.Team
{
    public class PlayerOnPosition
    {
        public string id { get; set; }
        public FieldPlayer player { get; set; }
        public string position { get; set; }
        
         public static IQuery<PlayerOnPosition> GetQuery(IQuery<PlayerOnPosition> query)
         {
             query.AddField(p => p.id)
                 .AddField(p => p.player, FieldPlayer.GetQuery)
                 .AddField(p => p.position);
             
             return query;
        }
    }
}