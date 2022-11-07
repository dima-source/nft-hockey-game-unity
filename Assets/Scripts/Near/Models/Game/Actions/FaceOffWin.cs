namespace Near.Models.Game.Actions {
    public class FaceOffWin : Action {
        public string account_id { get; set; }
        public int player_number { get; set; }
        public string player_position { get; set; }
        
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
            return $"{color}{player_number} won the face-off";
        }
    }
}