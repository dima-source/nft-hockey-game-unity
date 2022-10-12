using GraphQL.Query.Builder;
using Newtonsoft.Json;

namespace Near.Models.Tokens.Players.Goalie
{
    public class Goalie : Player
    {
        [JsonIgnore]
        public GoalieStats Stats { get; set; }
        public string goalie_number { get; set; }
        
        public override void SetStats(string jsonStats)
        {
            Stats = JsonConvert.DeserializeObject<GoalieStats>(jsonStats);
        }
        
        public static IQuery<Goalie> GetQuery(IQuery<Goalie> query)
        {
            query.AddField(p => p.id)
                .AddField(p => p.img)
                .AddField(p => p.name)
                .AddField(p => p.goalie_number)
                .AddField(p => p.reality)
                .AddField(p => p.nationality)
                .AddField(p => p.birthday)
                .AddField(p => p.player_type)
                .AddField(p => p.number)
                .AddField(p => p.hand)
                .AddField(p => p.player_role)
                .AddField(p => p.stats);
            return query;
        }
    }
}