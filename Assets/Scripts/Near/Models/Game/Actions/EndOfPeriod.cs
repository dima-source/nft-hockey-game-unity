namespace Near.Models.Game.Actions {
    public class EndOfPeriod : Action {
        public int number { get; set; }
        
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor}Period {number} ended";
        }
    }
}