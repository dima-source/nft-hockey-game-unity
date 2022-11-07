namespace Near.Models.Game.Actions {
    public class Rebound : Action {
        public string account_id { get; set; }
        public int player_number { get; set; }
        public string player_position { get; set; }
        
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
            return $"{DefaultColor}rebound -> {color}{player_number} took the puck";
        }
    }
}