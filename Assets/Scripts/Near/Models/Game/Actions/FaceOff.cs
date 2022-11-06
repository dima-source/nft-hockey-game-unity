namespace Near.Models.Game.Actions {
    public class FaceOff {
        public string action_type { get; set; }
        public string account_id1 { get; set; }
        public int player_number1 { get; set; }
        public string player_position1 { get; set; }
        public string account_id2 { get; set; }
        public int player_number2 { get; set; }
        public string player_position2 { get; set; }
    }
}