namespace Near.Models.Game
{
    public class UserInGameInfo
    {
        public int ID { get; set; }
        public User user { get; set; }
        public Team.Team team { get; set; }
        public bool take_to_called { get; set; }
        public bool coach_speech_called { get; set; }
        public bool is_goalie_out { get; set; }
    }
}