namespace Near.Models.Game.Actions {
    public class PassCaught : Action {
        public string account_id { get; set; }
        public int from_player_number { get; set; }
        public string from { get; set; }
        public int to_player_number { get; set; }
        public string to { get; set; }
        public int caught_player_number { get; set; }
        public string caught_player_position { get; set; }
        
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
            return $"{color}{caught_player_number} caught a pass from {from_player_number}";
        }
    }
}