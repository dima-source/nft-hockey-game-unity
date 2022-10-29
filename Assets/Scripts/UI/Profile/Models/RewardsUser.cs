using GraphQL.Query.Builder;

namespace UI.Profile.Models
{
    public class RewardsUser
    {
        public string id { get; set; }
        public int points { get; set; }
        public int wins { get; set; }
        public int max_wins_in_line { get; set; }
        public int wins_in_line { get; set; }
        public int games { get; set; }
        public int players_sold { get; set; }
        public int referrals_count { get; set; }
        public int friends_count { get; set; }
        public bool already_set_team { get; set; }
    }
}