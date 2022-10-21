using GraphQL.Query.Builder;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game
{
    public class Event
    {
        public string id { get; set; }
        public UserInGameInfo user1 { get; set; }
        public UserInGameInfo user2 { get; set; }
        public FieldPlayer player_with_puck { get; set; }
        public string action { get; set; }
        public int zone_number { get; set; }
        private string time { get; set; }

        public static IQuery<Event> GetQuery(IQuery<Event> query)
        {
            query.AddField(e => e.id)
                .AddField(e => e.user1, UserInGameInfo.GetQuery)
                .AddField(e => e.user2, UserInGameInfo.GetQuery)
                .AddField(e => e.player_with_puck, FieldPlayer.GetQuery)
                .AddField(e => e.action)
                .AddField(e => e.zone_number)
                .AddField(e => e.time);
            
            return query;
        }
    }
}