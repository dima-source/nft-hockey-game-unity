namespace Near.Models.Game.Actions {
    public class Dump : Action {
        public string account_id { get; set; }
        public int from_player_number { get; set; }
        public string from { get; set; }
        public int to_player_number { get; set; }
        public string to { get; set; }
        
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
            return $"{color}Dump from {from_player_number} to {to_player_number}";
        }
    }
}