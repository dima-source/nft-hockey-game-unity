using GraphQL.Query.Builder;
using Newtonsoft.Json;

namespace Near.Models.Tokens.Players.FieldPlayer
{
    public class FieldPlayer : Player
    {
        [JsonIgnore]
        public FieldPlayerStats Stats { get; set; }
        public int team_work { get; set; }
        public string native_position { get; set; }

        public int  number_of_penalty_events { get; set; }

        public override void SetStats(string jsonStats)
        {
            Stats = JsonConvert.DeserializeObject<FieldPlayerStats>(jsonStats);
        }

        public static IQuery<FieldPlayer> GetQuery(IQuery<FieldPlayer> query)
        {
            // TODO:
            query.AddField(p => p.native_position);
            return query;
        }
    }
}