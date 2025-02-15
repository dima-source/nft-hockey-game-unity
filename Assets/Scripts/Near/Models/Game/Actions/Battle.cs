namespace Near.Models.Game.Actions
{
    public class Battle : Action
    {
        public string account_id1 { get; set; }
        public int player_number1 { get; set; }
        public string player_position1 { get; set; }
        public string account_id2 { get; set; }
        public int player_number2 { get; set; }
        public string player_position2 { get; set; }

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
            return $"{UserColor}{userPlayerId}{DefaultColor} Battle {OpponentColor}{opponentPlayerId}";
        }
    }
}