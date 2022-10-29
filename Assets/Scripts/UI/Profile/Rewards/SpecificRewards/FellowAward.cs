using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class FellowAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Fellow award";
        protected override string Description => "Got twenty friends";

        public override bool IsObtained(RewardsUser user)
        {
            return user.friends_count >= 20;
        }
    }
}