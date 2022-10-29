using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class ProAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Pro award";
        protected override string Description => "Twenty victories";

        public override bool IsObtained(RewardsUser user)
        {
            return user.wins >= 20;
        }
    }
}