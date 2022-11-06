namespace Near.Models.Game.Actions {
    public class PenaltyShot {
        public string action_type { get; set; }
        public string account_id1 { get; set; }
        public string player_name { get; set; }
        public string player_img { get; set; }
        public int player_number { get; set; }
        public string account_id2 { get; set; }
        public string goalie_name { get; set; }
        public string goalie_img { get; set; }
        public int goalie_number { get; set; }
    }
}