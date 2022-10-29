using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class JuniorAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Junior award";
        protected override string Description => "Hundred games played";

        public override bool IsObtained(RewardsUser user)
        {
            return user.games >= 100;
        }
    }
}