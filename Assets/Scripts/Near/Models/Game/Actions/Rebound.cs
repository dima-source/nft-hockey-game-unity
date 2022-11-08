namespace Near.Models.Game.Actions {
    public class Rebound : Action {
        public string account_id { get; set; }
        
        // The player who received the puck
        public int player_number { get; set; }
        public string player_position { get; set; }
        
        public int goalie_number { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id)
            {
                return ColorizeMessage(OpponentColor, UserColor);
            }
            
            return ColorizeMessage(UserColor, OpponentColor);
        }

        private string ColorizeMessage(string userColor, string opponentColor)
        {
            return $"{userColor}{goalie_number} rebound\n" +
                   $"{opponentColor}{player_number} took the puck";
        }
    }
}