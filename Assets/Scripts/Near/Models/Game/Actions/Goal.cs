namespace Near.Models.Game.Actions {
    public class Goal : Action {
        public string account_id { get; set; }
        public string player_name1 { get; set; }
        public string player_img { get; set; }
        public int player_number1 { get; set; }
        public string player_name2 { get; set; }
        public string player_number2 { get; set; }
        
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
            string result = $"{color}{player_name1} scored a goal";
            if (player_number2 != null) 
            {
                result += $", {player_number2} assist";
            }

            return result;
        }
    }
}