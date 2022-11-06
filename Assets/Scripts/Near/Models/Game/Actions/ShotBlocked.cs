namespace Near.Models.Game.Actions {
    public class ShotBlocked {
        public string action_type { get; set; }
        public string account_id { get; set; }
        public int player_number { get; set; }
        public string player_position { get; set; }
    }
}