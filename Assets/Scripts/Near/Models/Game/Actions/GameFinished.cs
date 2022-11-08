namespace Near.Models.Game.Actions {
    public class GameFinished : Action {
        public string winner_account_id { get; set; }
        public string reward { get; set; }
        
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor}Game finished";
        }
    }
}