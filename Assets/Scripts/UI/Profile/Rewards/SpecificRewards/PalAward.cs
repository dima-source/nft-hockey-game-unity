using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class PalAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Pal award";
        protected override string Description => "Got ten friends";

        public override bool IsObtained(RewardsUser user)
        {
            return user.friends_count >= 10;
        }
    }
}