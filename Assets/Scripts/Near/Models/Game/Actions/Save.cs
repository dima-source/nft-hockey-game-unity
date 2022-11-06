namespace Near.Models.Game.Actions {
    public class Save {
        public string action_type { get; set; }
        public string account_id { get; set; }
        public int goalie_number { get; set; }
    }
}