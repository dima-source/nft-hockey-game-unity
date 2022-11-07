namespace Near.Models.Game.Actions {
    public class CoachSpeech : Action {
        public string account_id { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id)
            {
                return ColorizeMessage(UserColor);
            }
            
            return ColorizeMessage(UserColor);
        }

        private string ColorizeMessage(string color)
        {
            return $"{color} Coach speech";
        }
    }
}