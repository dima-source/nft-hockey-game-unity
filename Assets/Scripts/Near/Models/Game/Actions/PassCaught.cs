namespace Near.Models.Game.Actions {
    public class PassCaught {
        public string action_type { get; set; }
        public string account_id { get; set; }
        public int from_player_number { get; set; }
        public string from { get; set; }
        public int to_player_number { get; set; }
        public string to { get; set; }
        public int caught_player_number { get; set; }
        public string caught_player_position { get; set; }
    }
}