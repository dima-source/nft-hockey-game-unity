namespace Near.Models.Game.Actions {
    public class ShotBlocked : Action {
        public string account_id { get; set; }
        
        // The player who received the puck
        private string opponent_number { get; set; }
        private string opponent_position { get; set; }
        
        // The player who shot
        public int player_number { get; set; }
        
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id)
            {
                return ColorizeMessage(UserColor, OpponentColor);
            }
            
            return ColorizeMessage(OpponentColor, UserColor);
        }

        private string ColorizeMessage(string userColor, string opponentColor)
        {
            return $"{userColor}{player_number} shot\n" +
                   $"{opponentColor}{opponentColor} shot blocked";
        }
    }
}