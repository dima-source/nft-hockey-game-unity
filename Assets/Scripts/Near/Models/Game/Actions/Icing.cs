namespace Near.Models.Game.Actions {
    public class Icing : Action {
        public string account_id { get; set; }
        public int player_number { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id)
            {
                return ColorizeMessage(UserColor);
            }

            return ColorizeMessage(OpponentColor);
        }

        public string ColorizeMessage(string color)
        {
            return $"{color}{player_number} icing";
        }
    }
}