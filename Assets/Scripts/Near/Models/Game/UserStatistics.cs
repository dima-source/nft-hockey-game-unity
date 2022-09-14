namespace Near.Models.Game
{
    public class UserStatistics
    {
        public int id { get;set;}
        public int victories { get; set; }
        public int losses { get; set; }
        public int total_reward { get; set; }
        public int total_loss{ get; set; }
        public int total_goals { get; set; }
        public int total_misses { get; set; }
    }
}