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
    }
}