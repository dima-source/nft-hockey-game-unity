using GraphQL.Query.Builder;

namespace UI.Profile.Models
{
    public class RewardsUser
    {
        public string id { get; set; }
        public int points { get; set; } = 0;
        public int wins { get; set; } = 0;
        public int max_wins_in_line { get; set; } = 0;
        public int wins_in_line { get; set; } = 0;
        public int games { get; set; } = 0;
        public int players_sold { get; set; } = 0;
        public int referrals_count { get; set; } = 0;
        public int friends_count { get; set; } = 0;
        public bool already_set_team { get; set; } = false;
    }
}