namespace Near.Models.Game.Actions {
    public class GameFinished {
        public string action_type { get; set; }
        public string winner_account_id { get; set; }
        public string reward { get; set; }
    }
}