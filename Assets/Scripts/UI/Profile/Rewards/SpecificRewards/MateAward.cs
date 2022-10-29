using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class MateAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Mate award";
        protected override string Description => "Got five friends";

        public override bool IsObtained(RewardsUser user)
        {
            return user.friends_count >= 5;
        }
    }
}