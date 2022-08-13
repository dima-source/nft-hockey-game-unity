namespace Near.Models.Game
{
    public class UserInGameInfo
    {
        public int id { get; set; }
        public User user { get; set; }
        public GameData game { get; set; }
        public bool take_to_called { get; set; }
        public bool coach_speech_called { get; set; }
        public bool is_goalie_out { get; set; }
        
        
    }
}