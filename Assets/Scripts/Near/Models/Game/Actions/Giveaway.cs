namespace Near.Models.Game.Actions {
    public class Giveaway : Action {
        public string account_id1 { get; set; }
        
        // player who lost puck
        public int player_number1 { get; set; }
        public string player_position1 { get; set; }
        public string account_id2 { get; set; }
        
        // the player who took the puck
        public int player_number2 { get; set; }
        public string player_position2 { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id1)
            {
                return ColorizeMessage(player_number1, player_number2);
            }
            
            return ColorizeMessage(player_number1, player_number2);
        }

        private string ColorizeMessage(int userPlayerNumber, int opponentPlayerNumber)
        {
            return $"{UserColor}{userPlayerNumber} lost the puck \n" +
                   $"{OpponentColor}{opponentPlayerNumber} took the puck";
        }
    }
}