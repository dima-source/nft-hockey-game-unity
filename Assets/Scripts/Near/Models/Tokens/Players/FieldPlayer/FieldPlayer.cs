using GraphQL.Query.Builder;
using Near.Models.Game;
using Newtonsoft.Json;

namespace Near.Models.Tokens.Players.FieldPlayer
{
    public class FieldPlayer : Player
    {
        [JsonIgnore]
        public FieldPlayerStats Stats { get; set; }
        public float teamwork { get; set; }
        public string native_position { get; set; }
        public UserInGameInfo user_in_game_info { get; set; }
        public int  number_of_penalty_events { get; set; }

        public override void SetStats(string jsonStats)
        {
            Stats = JsonConvert.DeserializeObject<FieldPlayerStats>(jsonStats);
        }

        public static IQuery<FieldPlayer> GetQuery(IQuery<FieldPlayer> query)
        {
            query.AddField(p => p.id)
                .AddField(p => p.img)
                .AddField(p => p.user_in_game_info,
                    sq => 
                        sq.AddField(u => u.id))
                .AddField(p => p.name)
                .AddField(p => p.teamwork)
                .AddField(p => p.reality)
                .AddField(p => p.nationality)
                .AddField(p => p.birthday)
                .AddField(p => p.player_type)
                .AddField(p => p.number_of_penalty_events)
                .AddField(p => p.number)
                .AddField(p => p.hand)
                .AddField(p => p.player_role)
                .AddField(p => p.native_position)
                .AddField(p => p.stats);
            return query;
        }
    }
}