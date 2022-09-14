using GraphQL.Query.Builder;


namespace Near.Models.Game
{
    public class UserInGameInfo
    {
        public int id { get; set; }
        public User user { get; set; }
        public Team.Team team { get; set; }
        public bool take_to_called { get; set; }
        public bool coach_speech_called { get; set; }
        public bool is_goalie_out { get; set; }

        public static IQuery<UserInGameInfo> GetQuery(IQuery<UserInGameInfo> query)
        {
            query.AddField(u => u.id)
                .AddField(u => u.user, sq => sq.AddField(u => u.id))
                .AddField(u => u.team) // TODO
                .AddField(u => u.take_to_called)
                .AddField(u => u.coach_speech_called)
                .AddField(u => u.is_goalie_out);
            
            return query;
        }
    }
}