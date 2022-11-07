namespace Near.Models.Game.Actions {
    public class Takeaway : Action {
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
                return ColorizeMessage(UserColor, OpponentColor);
            }
            
            return ColorizeMessage(OpponentColor, UserColor);
        }

        private string ColorizeMessage(string color1, string color2)
        {
            return $"{color1}{player_number2} takeaway {color2}{player_number1}";
        }
    }
}