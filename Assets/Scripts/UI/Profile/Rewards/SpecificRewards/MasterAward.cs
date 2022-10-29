using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class MasterAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Master Award";
        protected override string Description => "Ten victories";

        public override bool IsObtained(RewardsUser user)
        {
            return user.wins >= 10;
        }
    }
}