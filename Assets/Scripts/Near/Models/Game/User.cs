using System.Collections.Generic;

namespace Near.Models.Game
{
    public class User
    {
        public string id { get; set; }
        public bool is_available { get; set; }
        public UserStatistics statistics { get; set; }
        public Team.Team team { get; set; }
        public string deposit { get; set; }
        public List<string> tokens { get; set; }
        public List<GameData> games { get; set; }
        public List<User>  friends { get; set; }
        public List<User> sent_friend_requests { get; set; }
        public List<User> friend_requests_received { get; set; }
        public List<AccountWithDeposit> sent_requests_play { get; set; }
        public List<AccountWithDeposit> requests_play_received { get; set; }
    }
}