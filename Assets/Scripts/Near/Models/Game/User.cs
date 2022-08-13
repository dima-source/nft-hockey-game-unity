using System.Collections.Generic;

namespace Near.Models.Game
{
    public class User
    {
        public int id { get; set; }
        public bool is_available { get; set; }
        public UserStatistics statistics { get; set; }
        public List<GameData> gameDatas { get; set; }
    }
}