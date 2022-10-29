using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class SeniorAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Senior award";
        protected override string Description => "Five hundred games played";

        public override bool IsObtained(RewardsUser user)
        {
            return user.games >= 10;
        }
    }
}