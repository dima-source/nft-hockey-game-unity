namespace Near.Models.Game.Actions {
    public class Hit : Action {
        public string account_id { get; set; }
        public int player_number { get; set; }
        public string player_position { get; set; }
        public int opponent_number { get; set; }
        
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
            return $"{userColor}{player_number} hit {opponentColor}{opponent_number}";
        }
    }
}