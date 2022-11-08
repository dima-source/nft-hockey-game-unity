namespace Near.Models.Game.Actions {
    public class CoachSpeech : Action {
        public string account_id { get; set; }
        
        public override string GetMessage(string accountId)
        {
            return "";
        }
    }
}