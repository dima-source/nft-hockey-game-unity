using System;
using GraphQL.Query.Builder;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players.FieldPlayer;

namespace Near.Models.Game
{
    public class Event
    {
        public string id { get; set; }
        public int event_number { get; set; }
        public UserInGameInfo user1 { get; set; }
        public UserInGameInfo user2 { get; set; }
        public FieldPlayer player_with_puck { get; set; }
        public string action { get; set; }
        public int zone_number { get; set; }
        private string time { get; set; }

        public static IQuery<Event> GetQuery(IQuery<Event> query)
        {
            EventPagination eventPagination = new EventPagination()
            {
                orderBy = "event_number"
            };
            
            query.AddArguments(eventPagination)
                .AddField(e => e.id)
                .AddField(e => e.event_number)
                .AddField(e => e.user1, UserInGameInfo.GetQuery)
                .AddField(e => e.user2, UserInGameInfo.GetQuery)
                .AddField(e => e.player_with_puck, FieldPlayer.GetQuery)
                .AddField(e => e.action)
                .AddField(e => e.zone_number)
                .AddField(e => e.time);
            
            return query;
        }

        public string FormattedAction(string playerId)
        {
            return action switch
            {
                "StartGame" => "Start game",
                "EndOfPeriod" => "End of period",
                "Overtime" => "Overtime",
                "GameOver" => "GAME OVER",
                
                _ => throw new Exception("Action not found. Event number: " + event_number)
            };
        }
    }
}