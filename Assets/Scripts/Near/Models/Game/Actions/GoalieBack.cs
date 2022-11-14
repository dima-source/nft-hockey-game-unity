namespace Near.Models.Game.Actions {
    public class GoalieBack : Action {
        public string account_id { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id)
            {
                return ColorizeMessage(UserColor);
            }
            
            return ColorizeMessage(OpponentColor);
        }

        private string ColorizeMessage(string color)
        {
            return $"{color}goalie in the net";
        }
    }
}