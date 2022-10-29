using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class RookieAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Rookie award";
        protected override string Description => "First victory";

        public override bool IsObtained(RewardsUser user)
        {
            return user.wins >= 1;
        }
    }
}