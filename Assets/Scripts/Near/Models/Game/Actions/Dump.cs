namespace Near.Models.Game.Actions {
    public class Dump : Action {
        public string account_id { get; set; }
        public int zone_number { get; set; }
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
            string result = $"{color}{from_player_number}dump";
            if (zone_number == 2)
            {
                result += "-out of the zone";
            }
            else
            {
                result += "-in of the zone";
            }

            result += $"\n{to_player_number} chases the puck";
            return result;
        }
    }
}