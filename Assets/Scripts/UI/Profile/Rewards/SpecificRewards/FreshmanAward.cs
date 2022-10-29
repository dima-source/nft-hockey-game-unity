using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class FreshmanAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Freshman award";
        protected override string Description => "Twenty games played";

        public override bool IsObtained(RewardsUser user)
        {
            return user.games >= 20;
        }
    }
}