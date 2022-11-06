namespace Near.Models.Game.Actions {
    public class FightWon {
        public string action_type { get; set; }
        public string account_id { get; set; }
        public string player_name { get; set; }
        public string player_img { get; set; }
        public int player_number { get; set; }
    }
}