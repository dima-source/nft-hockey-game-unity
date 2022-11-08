namespace Near.Models.Game.Actions {
    public class Fight : Action {
        public string account_id1 { get; set; }
        public int zone_number { get; set; }
        public int player_number1 { get; set; }
        public string account_id2 { get; set; }
        public int player_number2 { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id1)
            {
                return ColorizeMessage(player_number1, player_number1);
            }
            
            return ColorizeMessage(player_number2, player_number2);
        }

        private string ColorizeMessage(int userPlayerId, int opponentPlayerId)
        {
            return $"{UserColor}{userPlayerId}{DefaultColor} fight {OpponentColor}{opponentPlayerId}";
        }
    }
}