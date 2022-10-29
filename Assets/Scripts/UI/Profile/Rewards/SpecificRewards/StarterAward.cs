using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class StarterAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Started award";
        protected override string Description => "Ten games played";

        public override bool IsObtained(RewardsUser user)
        {
            return user.games >= 10;
        }
    }
}