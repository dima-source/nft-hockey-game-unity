namespace Near.Models.Game.Actions {
    public class DelayedPenaltySignal : Action {
        public string type_of_penalty { get; set; }
        
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor} The referee ruled the violation";
        }
    }
}