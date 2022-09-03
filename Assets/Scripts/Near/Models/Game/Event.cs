using GraphQL.Query.Builder;
using Unity.VisualScripting;

namespace Near.Models.Game
{
    public class Event
    {
        public int id { get; set; }
        public UserInGameInfo user1 { get; set; }
        public UserInGameInfo user2 { get; set; }
        public string player_with_puck { get; set; }
        public string action { get; set; }
        public int zone_number { get; set; }
        private int time { get; set; }

        public static IQuery<Event> GetQuery(IQuery<Event> query)
        {
            query.AddField(e => e.id)
                .AddField(e => e.user1, UserInGameInfo.GetQuery)
                .AddField(e => e.user2, UserInGameInfo.GetQuery)
                .AddField(e => e.player_with_puck)
                .AddField(e => e.action)
                .AddField(e => e.zone_number)
                .AddField(e => e.time);
            
            return query;
        }
    }
}