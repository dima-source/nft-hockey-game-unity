namespace Near.Models.Game.Actions {
    public class PenaltyShot : Action {
        public string account_id1 { get; set; }
        public string player_name { get; set; }
        public string player_img { get; set; }
        public int player_number { get; set; }
        public string account_id2 { get; set; }
        public string goalie_name { get; set; }
        public string goalie_img { get; set; }
        public int goalie_number { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id1)
            {
                return ColorizeMessage(UserColor);
            }
            
            return ColorizeMessage(UserColor);
        }

        private string ColorizeMessage(string color)
        {
            return $"{color}{player_number} penalty shot";
        }
    }
}