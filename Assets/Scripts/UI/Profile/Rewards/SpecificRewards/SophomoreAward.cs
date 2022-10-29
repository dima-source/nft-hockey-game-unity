using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class SophomoreAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Sophomore award";
        protected override string Description => "Fifty games played";

        public override bool IsObtained(RewardsUser user)
        {
            return user.games >= 50;
        }
    }
}