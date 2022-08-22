using System.Collections.Generic;

namespace Near.Models.Game
{
    public class User
    {
        public string id { get; set; }
        public bool is_available { get; set; }
        public UserStatistics statistics { get; set; }
        public Team.Team team { get; set; }
        public List<string> tokens { get; set; }
        public List<GameData> games { get; set; }
    }
}