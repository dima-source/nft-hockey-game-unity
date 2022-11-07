namespace Near.Models.Game.Actions {
    public class Penalty : Action {
        public string account_id { get; set; }
        public string player_img { get; set; }
        public string player_name { get; set; }
        public int player_number { get; set; }
        
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
            return $"{color} penalty for {player_number}";
        }
    }
}